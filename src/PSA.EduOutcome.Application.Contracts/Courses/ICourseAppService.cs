using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using PSA.EduOutcome.Courses.Dtos;

namespace PSA.EduOutcome.Courses
{
    public interface ICourseAppService : 
        ICrudAppService<
            CourseDto,
            Guid,
            GetCourseListDto,
            CreateCourseDto,
            UpdateCourseDto>
    {
        Task<CourseDto> GetByCodeAsync(string code);
        Task<List<CourseDto>> GetByProgramAsync(Guid programId);
        Task<List<CourseDto>> GetByFacultyAsync(Guid facultyId);
        Task<CourseDto> ActivateAsync(Guid id);
        Task<CourseDto> DeactivateAsync(Guid id);
        Task<CourseDto> ArchiveAsync(Guid id);
        Task<byte[]> ExportToExcelAsync(GetCourseListDto input);
        Task ImportFromExcelAsync(byte[] fileContent);
        Task<CourseStatisticsDto> GetStatisticsAsync(Guid id);
        Task<List<CourseDto>> GetPrerequisitesAsync(Guid id);
        Task<List<CourseDto>> GetElectiveCoursesAsync(Guid programId, string semester);
    }
} 