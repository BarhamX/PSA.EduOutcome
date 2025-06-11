using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
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
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<StudentAppService> _logger;

        public StudentAppService(
            IStudentRepository studentRepository,
            ILogger<StudentAppService> logger) 
            : base(studentRepository)
        {
            _studentRepository = studentRepository;
            _logger = logger;

            GetPolicyName = EduOutcomePermissions.Students.Default;
            GetListPolicyName = EduOutcomePermissions.Students.Default;
            CreatePolicyName = EduOutcomePermissions.Students.Create;
            UpdatePolicyName = EduOutcomePermissions.Students.Edit;
            DeletePolicyName = EduOutcomePermissions.Students.Delete;
        }

        public async Task<StudentDto> GetByStudentNumberAsync(string studentNumber)
        {
            var student = await _studentRepository.GetByStudentNumberAsync(studentNumber);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        public async Task<List<StudentDto>> GetByProgramAsync(Guid programId)
        {
            var students = await _studentRepository.GetListByProgramAsync(programId);
            return ObjectMapper.Map<List<Student>, List<StudentDto>>(students);
        }

        public async Task<StudentDto> GetByUserIdAsync(Guid userId)
        {
            var student = await _studentRepository.GetByUserIdAsync(userId);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        public async Task<StudentDto> GraduateAsync(Guid id)
        {
            var student = await _studentRepository.GetAsync(id);
            student.Graduate();
            await _studentRepository.UpdateAsync(student);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        public async Task<StudentDto> SuspendAsync(Guid id)
        {
            var student = await _studentRepository.GetAsync(id);
            student.Suspend();
            await _studentRepository.UpdateAsync(student);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        public async Task<StudentDto> WithdrawAsync(Guid id)
        {
            var student = await _studentRepository.GetAsync(id);
            student.Withdraw();
            await _studentRepository.UpdateAsync(student);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        public async Task<StudentDto> ReactivateAsync(Guid id)
        {
            var student = await _studentRepository.GetAsync(id);
            student.Reactivate();
            await _studentRepository.UpdateAsync(student);
            return ObjectMapper.Map<Student, StudentDto>(student);
        }

        public async Task<byte[]> ExportToExcelAsync(GetStudentListDto input)
        {
            var students = await _studentRepository.GetListAsync(
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

        public async Task<StudentStatisticsDto> GetStatisticsAsync(Guid id)
        {
            var student = await _studentRepository.GetWithDetailsAsync(id);
            
            return new StudentStatisticsDto
            {
                TotalCourses = student.Enrollments.Count,
                CompletedCourses = student.Enrollments.Count(e => e.Status == "Completed"),
                AverageGrade = student.Enrollments
                    .Where(e => e.Grade.HasValue)
                    .Average(e => e.Grade.Value),
                LearningOutcomesAchieved = student.Responses
                    .Count(r => r.IsCorrect)
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