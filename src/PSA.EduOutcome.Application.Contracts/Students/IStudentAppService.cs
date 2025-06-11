using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using PSA.EduOutcome.Students.Dtos;

namespace PSA.EduOutcome.Students
{
    public interface IStudentAppService : 
        ICrudAppService<
            StudentDto,
            Guid,
            GetStudentListDto,
            CreateStudentDto,
            UpdateStudentDto>
    {
        Task<StudentDto> GetByStudentNumberAsync(string studentNumber);
        Task<List<StudentDto>> GetByProgramAsync(Guid programId);
        Task<StudentDto> GetByUserIdAsync(Guid userId);
        Task<StudentDto> GraduateAsync(Guid id);
        Task<StudentDto> SuspendAsync(Guid id);
        Task<StudentDto> WithdrawAsync(Guid id);
        Task<StudentDto> ReactivateAsync(Guid id);
        Task<byte[]> ExportToExcelAsync(GetStudentListDto input);
        Task ImportFromExcelAsync(byte[] fileContent);
        Task<StudentStatisticsDto> GetStatisticsAsync(Guid id);
    }
} 