using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using PSA.EduOutcome.Courses.Dtos;
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
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CourseAppService> _logger;

        public CourseAppService(
            ICourseRepository courseRepository,
            ILogger<CourseAppService> logger) 
            : base(courseRepository)
        {
            _courseRepository = courseRepository;
            _logger = logger;

            GetPolicyName = EduOutcomePermissions.Courses.Default;
            GetListPolicyName = EduOutcomePermissions.Courses.Default;
            CreatePolicyName = EduOutcomePermissions.Courses.Create;
            UpdatePolicyName = EduOutcomePermissions.Courses.Edit;
            DeletePolicyName = EduOutcomePermissions.Courses.Delete;
        }

        public async Task<CourseDto> GetByCodeAsync(string code)
        {
            var course = await _courseRepository.GetByCodeAsync(code);
            return ObjectMapper.Map<Course, CourseDto>(course);
        }

        public async Task<List<CourseDto>> GetByProgramAsync(Guid programId)
        {
            var courses = await _courseRepository.GetListByProgramAsync(programId);
            return ObjectMapper.Map<List<Course>, List<CourseDto>>(courses);
        }

        public async Task<List<CourseDto>> GetByFacultyAsync(Guid facultyId)
        {
            var courses = await _courseRepository.GetListByFacultyAsync(facultyId);
            return ObjectMapper.Map<List<Course>, List<CourseDto>>(courses);
        }

        public async Task<CourseDto> ActivateAsync(Guid id)
        {
            var course = await _courseRepository.GetAsync(id);
            course.Activate();
            await _courseRepository.UpdateAsync(course);
            return ObjectMapper.Map<Course, CourseDto>(course);
        }

        public async Task<CourseDto> DeactivateAsync(Guid id)
        {
            var course = await _courseRepository.GetAsync(id);
            course.Deactivate();
            await _courseRepository.UpdateAsync(course);
            return ObjectMapper.Map<Course, CourseDto>(course);
        }

        public async Task<CourseDto> ArchiveAsync(Guid id)
        {
            var course = await _courseRepository.GetAsync(id);
            course.Archive();
            await _courseRepository.UpdateAsync(course);
            return ObjectMapper.Map<Course, CourseDto>(course);
        }

        public async Task<byte[]> ExportToExcelAsync(GetCourseListDto input)
        {
            var courses = await _courseRepository.GetListAsync(
                input.Filter,
                input.Sorting,
                input.MaxResultCount,
                input.SkipCount
            );

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
            var course = await _courseRepository.GetWithDetailsAsync(id);
            
            return new CourseStatisticsDto
            {
                TotalEnrollments = course.Enrollments.Count,
                ActiveEnrollments = course.Enrollments.Count(e => e.Status == "Active"),
                AverageGrade = course.Enrollments
                    .Where(e => e.FinalGrade.HasValue)
                    .Select(e => e.FinalGrade.Value)
                    .DefaultIfEmpty(0)
                    .Average(),
                LearningOutcomesCovered = course.Questions
                    .SelectMany(q => q.LearningOutcomes)
                    .Distinct()
                    .Count()
            };
        }

        public async Task<List<CourseDto>> GetPrerequisitesAsync(Guid id)
        {
            var course = await _courseRepository.GetWithDetailsAsync(id);
            var prerequisites = await _courseRepository.GetPrerequisitesAsync(course);
            return ObjectMapper.Map<List<Course>, List<CourseDto>>(prerequisites);
        }

        public async Task<List<CourseDto>> GetElectiveCoursesAsync(Guid programId, string semester)
        {
            var courses = await _courseRepository.GetElectiveCoursesAsync(programId, semester);
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