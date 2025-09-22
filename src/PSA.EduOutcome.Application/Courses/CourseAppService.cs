using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using PSA.EduOutcome.Courses.Dtos;
using PSA.EduOutcome.Domain.Services;
using PSA.EduOutcome.Entities;
using PSA.EduOutcome.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace PSA.EduOutcome.Courses
{
    [Authorize]
    public class CourseAppService :
        CrudAppService<
            Course,
            CourseDto,
            Guid,
            GetCourseListDto,
            CreateCourseDto,
            UpdateCourseDto>,
        ICourseAppService
    {
        private readonly ILogger<CourseAppService> _logger;
        private readonly IRepository<Enrollment, Guid> _enrollmentRepository;
        private readonly ILearningOutcomeCalculationService _learningOutcomeCalculationService;
        private readonly IRepository<LearningOutcome, Guid> _learningOutcomeRepository;

        public CourseAppService(
            IRepository<Course, Guid> repository,
            ILogger<CourseAppService> logger,
            IRepository<Enrollment, Guid> enrollmentRepository,
            ILearningOutcomeCalculationService learningOutcomeCalculationService,
            IRepository<LearningOutcome, Guid> learningOutcomeRepository)
            : base(repository)
        {
            _logger = logger;
            _enrollmentRepository = enrollmentRepository;
            _learningOutcomeCalculationService = learningOutcomeCalculationService;
            _learningOutcomeRepository = learningOutcomeRepository;

            GetPolicyName = EduOutcomePermissions.Courses.Default;
            GetListPolicyName = EduOutcomePermissions.Courses.Default;
            CreatePolicyName = EduOutcomePermissions.Courses.Create;
            UpdatePolicyName = EduOutcomePermissions.Courses.Edit;
            DeletePolicyName = EduOutcomePermissions.Courses.Delete;
        }

        public async Task<CourseDto> GetByCodeAsync(string code)
        {
            var course = await Repository.FirstOrDefaultAsync(c => c.Code == code);
            return ObjectMapper.Map<Course, CourseDto>(course);
        }

        public async Task<List<CourseDto>> GetByProgramAsync(Guid programId)
        {
            var courses = await Repository.GetListAsync(c => c.ProgramId == programId);
            return ObjectMapper.Map<List<Course>, List<CourseDto>>(courses);
        }

        public async Task<List<CourseDto>> GetByFacultyAsync(Guid facultyId)
        {
            // Note: This assumes Course has a FacultyId property or navigation
            // If not available, we'll need to join through Program->Faculty
            var courses = await Repository.GetListAsync(); // TODO: Add proper faculty filtering
            return ObjectMapper.Map<List<Course>, List<CourseDto>>(courses);
        }

        public async Task<CourseDto> ActivateAsync(Guid id)
        {
            var course = await Repository.GetAsync(id);
            course.Activate();
            await Repository.UpdateAsync(course);
            return ObjectMapper.Map<Course, CourseDto>(course);
        }

        public async Task<CourseDto> DeactivateAsync(Guid id)
        {
            var course = await Repository.GetAsync(id);
            course.Deactivate();
            await Repository.UpdateAsync(course);
            return ObjectMapper.Map<Course, CourseDto>(course);
        }

        public async Task<CourseDto> ArchiveAsync(Guid id)
        {
            var course = await Repository.GetAsync(id);
            course.Archive();
            await Repository.UpdateAsync(course);
            return ObjectMapper.Map<Course, CourseDto>(course);
        }

        public async Task<byte[]> ExportToExcelAsync(GetCourseListDto input)
        {
            var courses = await Repository.GetListAsync();

            // TODO: Implement Excel export using a library like EPPlus or ClosedXML
            throw new NotImplementedException("Excel export not implemented yet");
        }

        public async Task ImportFromExcelAsync(byte[] fileContent)
        {
            // TODO: Implement Excel import using a library like EPPlus or ClosedXML
            throw new NotImplementedException("Excel import not implemented yet");
        }

        public async Task<CourseStatisticsDto> GetStatisticsAsync(Guid id)
        {
            var course = await Repository.GetAsync(id);

            var enrollmentQueryable = await _enrollmentRepository.GetQueryableAsync();
            var courseEnrollmentsQuery = enrollmentQueryable.Where(e => e.CourseId == id);

            var totalEnrollments = await AsyncExecuter.CountAsync(courseEnrollmentsQuery);
                        var activeEnrollments = await AsyncExecuter.CountAsync(courseEnrollmentsQuery.Where(e => e.Status == EnrollmentStatus.Enrolled));
                        var enrollments = await AsyncExecuter.ToListAsync(courseEnrollmentsQuery);

            var gradedEnrollments = enrollments.Where(e => e.FinalGrade.HasValue).ToList();
            var averageGrade = gradedEnrollments.Count > 0
                ? Math.Round(gradedEnrollments.Average(e => e.FinalGrade!.Value), 2, MidpointRounding.AwayFromZero)
                : 0m;

            var learningOutcomes = await _learningOutcomeRepository.GetListAsync(lo => lo.CourseId == id && lo.IsActive);
            var totalOutcomeWeight = learningOutcomes.Sum(lo => lo.MaxMark);

            var outcomeAccumulator = learningOutcomes.ToDictionary(lo => lo.Id, _ => new List<decimal>());
            var cumulativeCourseAchievement = 0m;
            var participantCount = 0;

            foreach (var enrollment in enrollments)
            {
                if (enrollment.Status == EnrollmentStatus.Withdrawn)
                {
                    continue;
                }

                var outcomeBreakdown = await _learningOutcomeCalculationService.CalculateStudentCourseOutcomesAsync(enrollment.StudentId, id);
                if (outcomeBreakdown.Count == 0 || learningOutcomes.Count == 0 || totalOutcomeWeight <= 0m)
                {
                    continue;
                }

                participantCount++;

                decimal weighted = 0m;
                foreach (var outcome in learningOutcomes)
                {
                    var percentage = outcomeBreakdown.TryGetValue(outcome.Id, out var value) ? value : 0m;
                    outcomeAccumulator[outcome.Id].Add(percentage);
                    weighted += (percentage / 100m) * outcome.MaxMark;
                }

                var courseAchievement = (weighted / totalOutcomeWeight) * 100m;
                cumulativeCourseAchievement += courseAchievement;
            }

            var averageOutcomeAchievement = participantCount > 0
                ? Math.Round(cumulativeCourseAchievement / participantCount, 2, MidpointRounding.AwayFromZero)
                : 0m;

            var learningOutcomeAchievements = learningOutcomes.Select(outcome =>
            {
                var entries = outcomeAccumulator.TryGetValue(outcome.Id, out var values) ? values : new List<decimal>();
                var average = entries.Count > 0
                    ? Math.Round(entries.Average(), 2, MidpointRounding.AwayFromZero)
                    : 0m;

                return new CourseLearningOutcomeAchievementDto
                {
                    LearningOutcomeId = outcome.Id,
                    Code = outcome.Code,
                    Description = outcome.Description,
                    AchievementPercentage = average
                };
            }).ToList();

            return new CourseStatisticsDto
            {
                TotalEnrollments = totalEnrollments,
                ActiveEnrollments = activeEnrollments,
                AverageGrade = averageGrade,
                LearningOutcomesCovered = learningOutcomes.Count,
                AverageOutcomeAchievement = averageOutcomeAchievement,
                LearningOutcomeAchievements = learningOutcomeAchievements
            };
        }

        public async Task<List<CourseDto>> GetPrerequisitesAsync(Guid id)
        {
            var course = await Repository.GetAsync(id);
            // TODO: Implement prerequisite logic when prerequisite relationships are properly defined
            var prerequisites = new List<Course>();
            return ObjectMapper.Map<List<Course>, List<CourseDto>>(prerequisites);
        }

        public async Task<List<CourseDto>> GetElectiveCoursesAsync(Guid programId, string semester)
        {
            var queryable = (await Repository.GetQueryableAsync())
                .Where(c => c.ProgramId == programId && c.IsElective && c.Status == "Active");

            if (!string.IsNullOrWhiteSpace(semester) && int.TryParse(semester, out var semesterValue))
            {
                queryable = queryable.Where(c => c.Semester == semesterValue);
            }

            var courses = await AsyncExecuter.ToListAsync(queryable.OrderBy(c => c.Name));
            return ObjectMapper.Map<List<Course>, List<CourseDto>>(courses);
        }

        protected override async Task<IQueryable<Course>> CreateFilteredQueryAsync(GetCourseListDto input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(c =>
                    c.Name.Contains(input.Filter) ||
                    c.Code.Contains(input.Filter) ||
                    c.Description.Contains(input.Filter)
                );
            }

            if (input.ProgramId.HasValue)
            {
                query = query.Where(c => c.ProgramId == input.ProgramId.Value);
            }

            if (input.FacultyId.HasValue)
            {
                query = query.Where(c => c.FacultyId == input.FacultyId.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.Status))
            {
                query = query.Where(c => c.Status == input.Status);
            }

            if (input.Semester.HasValue)
            {
                query = query.Where(c => c.Semester == input.Semester.Value);
            }

            if (input.AcademicYear.HasValue)
            {
                query = query.Where(c => c.AcademicYear == input.AcademicYear.Value);
            }

            return query;
        }
    }
}


