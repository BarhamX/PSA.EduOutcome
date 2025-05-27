using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PSA.EduOutcome.Entities;
using Volo.Abp.Domain.Repositories;

namespace PSA.EduOutcome.Domain.Repositories
{
    public interface IStudentRepository : IRepository<Student, Guid>
    {
        Task<Student> FindByStudentNumberAsync(string studentNumber);
        Task<List<Student>> GetStudentsByProgramAsync(Guid programId);
        Task<Student> FindByEmailAsync(string email);
    }
} 