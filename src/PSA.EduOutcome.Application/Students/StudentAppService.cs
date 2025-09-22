using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using PSA.EduOutcome.Domain.Services;
using PSA.EduOutcome.Entities;
using PSA.EduOutcome.Permissions;
using PSA.EduOutcome.Students.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace PSA.EduOutcome.Students
{
    [Authorize]
    public class StudentAppService :
        CrudAppService<
            Student,
            StudentDto,
            Guid,
            GetStudentListDto,
            CreateStudentDto,
            UpdateStudentDto>,
        IStudentAppService
    {
        private const decimal OutcomeAchievementThreshold = 70m;

        private readonly ILogger<StudentAppService> _logger;
        private readonly IRepository<Enrollment, Guid> _enrollmentRepository;
        private readonly ILearningOutcomeCalculationService _learningOutcomeCalculationService;
        private readonly IRepository<Course, Guid> _courseRepository;
        private readonly IRepository<LearningOutcome, Guid> _learningOutcomeRepository;

        public StudentAppService(
            IRepository<Student, Guid> repository,
            ILogger<StudentAppService> logger,
            IRepository<Enrollment, Guid> enrollmentRepository,
            ILearningOutcomeCalculationService learningOutcomeCalculationService,
            IRepository<Course, Guid> courseRepository,
            IRepository<LearningOutcome, Guid> learningOutcomeRepository)
            : base(repository)
        {
            _logger = logger;
            _enrollmentRepository = enrollmentRepository;
            _learningOutcomeCalculationService = learningOutcomeCalculationService;
            _courseRepository = courseRepository;
            _learningOutcomeRepository = learningOutcomeRepository;

            GetPolicyName = EduOutcomePermissions.Students.Default;
            GetListPolicyName = EduOutcomePermissions.Students.Default;
            CreatePolicyName = EduOutcomePermissions.Students.Create;
            UpdatePolicyName = EduOutcomePermissions.Students.Edit;
            DeletePolicyName = EduOutcomePermissions.Students.Delete;
        }

        public async Task<StudentDto> GetByStudentNumberAsync(string studentNumber)
        {
            var student = await Repository.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        public async Task<List<StudentDto>> GetByProgramAsync(Guid programId)
        {
            var students = await Repository.GetListAsync(s => s.ProgramId == programId);
            return ObjectMapper.Map<List<Student>, List<StudentDto>>(students);
        }

        public async Task<StudentDto> GetByUserIdAsync(Guid userId)
        {
            var student = await Repository.FirstOrDefaultAsync(s => s.UserId == userId);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        public async Task<StudentDto> GraduateAsync(Guid id)
        {
            var student = await Repository.GetAsync(id);
            student.Graduate();
            await Repository.UpdateAsync(student);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        public async Task<StudentDto> SuspendAsync(Guid id)
        {
            var student = await Repository.GetAsync(id);
            student.Suspend();
            await Repository.UpdateAsync(student);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        public async Task<StudentDto> WithdrawAsync(Guid id)
        {
            var student = await Repository.GetAsync(id);
            student.Withdraw();
            await Repository.UpdateAsync(student);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        public async Task<StudentDto> ReactivateAsync(Guid id)
        {
            var student = await Repository.GetAsync(id);
            student.Reactivate();
            await Repository.UpdateAsync(student);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        public async Task<byte[]> ExportToExcelAsync(GetStudentListDto input)
        {
            var students = await Repository.GetListAsync();

            // TODO: Implement Excel export using a library like EPPlus or ClosedXML
            throw new NotImplementedException("Excel export not implemented yet");
        }

        public async Task ImportFromExcelAsync(byte[] fileContent)
        {
            // TODO: Implement Excel import using a library like EPPlus or ClosedXML
            throw new NotImplementedException("Excel import not implemented yet");
        }

        public async Task<StudentStatisticsDto> GetStatisticsAsync(Guid id)
        {
            var student = await Repository.GetAsync(id);

            var enrollments = await _enrollmentRepository.GetListAsync(e => e.StudentId == id);
            var totalCourses = enrollments.Count;
            var completedCourses = enrollments.Count(e => e.Status == EnrollmentStatus.Completed || e.Status == EnrollmentStatus.Failed);
            var activeCourses = enrollments.Count(e => e.Status == EnrollmentStatus.Enrolled);

            var gradedEnrollments = enrollments.Where(e => e.FinalGrade.HasValue).ToList();
            var averageGrade = gradedEnrollments.Count > 0
                ? Math.Round(gradedEnrollments.Average(e => e.FinalGrade!.Value), 2, MidpointRounding.AwayFromZero)
                : 0m;

            var courseIds = enrollments.Select(e => e.CourseId).Distinct().ToList();
            var courses = courseIds.Count > 0
                ? await _courseRepository.GetListAsync(c => courseIds.Contains(c.Id))
                : new List<Course>();
            var courseById = courses.ToDictionary(c => c.Id);

            var learningOutcomes = courseIds.Count > 0
                ? await _learningOutcomeRepository.GetListAsync(lo => courseIds.Contains(lo.CourseId) && lo.IsActive)
                : new List<LearningOutcome>();
            var learningOutcomesByCourse = learningOutcomes
                .GroupBy(lo => lo.CourseId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var courseAchievements = new List<StudentCourseAchievementDto>();
            var totalOutcomeAchievement = 0m;
            var outcomeCourseCount = 0;
            var learningOutcomesAchieved = 0;

            foreach (var enrollment in enrollments)
            {
                if (!courseById.TryGetValue(enrollment.CourseId, out var course))
                {
                    continue;
                }

                if (!learningOutcomesByCourse.TryGetValue(enrollment.CourseId, out var courseLearningOutcomes) || courseLearningOutcomes.Count == 0)
                {
                    continue;
                }

                var outcomeBreakdown = await _learningOutcomeCalculationService.CalculateStudentCourseOutcomesAsync(id, enrollment.CourseId);
                if (outcomeBreakdown.Count == 0)
                {
                    continue;
                }

                var totalWeight = courseLearningOutcomes.Sum(lo => lo.MaxMark);
                if (totalWeight <= 0m)
                {
                    continue;
                }

                var learningOutcomeDetails = new List<StudentLearningOutcomeAchievementDto>();
                decimal weighted = 0m;

                foreach (var learningOutcome in courseLearningOutcomes)
                {
                    var percentage = outcomeBreakdown.TryGetValue(learningOutcome.Id, out var value) ? value : 0m;
                    weighted += (percentage / 100m) * learningOutcome.MaxMark;

                    if (percentage >= OutcomeAchievementThreshold)
                    {
                        learningOutcomesAchieved++;
                    }

                    learningOutcomeDetails.Add(new StudentLearningOutcomeAchievementDto
                    {
                        LearningOutcomeId = learningOutcome.Id,
                        Code = learningOutcome.Code,
                        Description = learningOutcome.Description,
                        AchievementPercentage = Math.Round(percentage, 2, MidpointRounding.AwayFromZero)
                    });
                }

                var courseAchievement = Math.Round((weighted / totalWeight) * 100m, 2, MidpointRounding.AwayFromZero);
                totalOutcomeAchievement += courseAchievement;
                outcomeCourseCount++;

                courseAchievements.Add(new StudentCourseAchievementDto
                {
                    CourseId = course.Id,
                    CourseName = course.Name,
                    CourseCode = course.Code,
                    AchievementPercentage = courseAchievement,
                    LearningOutcomeAchievements = learningOutcomeDetails
                });
            }

            var averageOutcomeAchievement = outcomeCourseCount > 0
                ? Math.Round(totalOutcomeAchievement / outcomeCourseCount, 2, MidpointRounding.AwayFromZero)
                : 0m;

            return new StudentStatisticsDto
            {
                TotalCourses = totalCourses,
                CompletedCourses = completedCourses,
                ActiveCourses = activeCourses,
                AverageGrade = averageGrade,
                LearningOutcomesAchieved = learningOutcomesAchieved,
                AverageOutcomeAchievement = averageOutcomeAchievement,
                CourseAchievements = courseAchievements
            };
        }

        protected override async Task<IQueryable<Student>> CreateFilteredQueryAsync(GetStudentListDto input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(s =>
                    s.FirstName.Contains(input.Filter) ||
                    s.LastName.Contains(input.Filter) ||
                    s.StudentNumber.Contains(input.Filter) ||
                    s.Email.Contains(input.Filter)
                );
            }

            if (input.ProgramId.HasValue)
            {
                query = query.Where(s => s.ProgramId == input.ProgramId.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.Status))
            {
                query = query.Where(s => s.Status == input.Status);
            }

            return query;
        }
    }
}
