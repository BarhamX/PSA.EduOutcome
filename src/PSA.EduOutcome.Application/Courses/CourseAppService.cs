using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using PSA.EduOutcome.Courses.Dtos;
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

        public CourseAppService(
            IRepository<Course, Guid> repository,
            ILogger<CourseAppService> logger) 
            : base(repository)
        {
            _logger = logger;

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
            
            // TODO: Implement statistics calculation when enrollment and question entities are available
            return new CourseStatisticsDto
            {
                TotalEnrollments = 0,
                ActiveEnrollments = 0,
                AverageGrade = 0,
                LearningOutcomesCovered = 0
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
            var courses = await Repository.GetListAsync(c => c.ProgramId == programId && c.IsElective);
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