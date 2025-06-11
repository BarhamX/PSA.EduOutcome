using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using PSA.EduOutcome.DTOs;
using PSA.EduOutcome.Entities;
using PSA.EduOutcome.Domain.Repositories;
using AutoMapper;

namespace PSA.EduOutcome
{
    public class StudentAppService : ApplicationService, IStudentAppService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentAppService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<StudentDto> GetAsync(Guid id)
        {
            var student = await _studentRepository.GetAsync(id);
            return _mapper.Map<Student, StudentDto>(student);
        }

        public async Task<PagedResultDto<StudentDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var count = await _studentRepository.GetCountAsync();
            var students = await _studentRepository.GetListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<StudentDto>(count, _mapper.Map<List<Student>, List<StudentDto>>(students));
        }

        public async Task<StudentDto> CreateAsync(CreateStudentDto input)
        {
            var student = Student.Create(
                input.FirstName,
                input.LastName,
                input.Email,
                input.DateOfBirth,
                input.Gender,
                input.ProgramId,
                input.ReferenceNumber,
                input.Phone
            );
            await _studentRepository.InsertAsync(student);
            return _mapper.Map<Student, StudentDto>(student);
        }

        public async Task<StudentDto> UpdateAsync(Guid id, UpdateStudentDto input)
        {
            var student = await _studentRepository.GetAsync(id);
            student.Update(
                input.FirstName,
                input.LastName,
                input.Email,
                input.Phone,
                input.DateOfBirth,
                input.Gender,
                input.ProgramId
            );
            await _studentRepository.UpdateAsync(student);
            return _mapper.Map<Student, StudentDto>(student);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _studentRepository.DeleteAsync(id);
        }
    }
} 