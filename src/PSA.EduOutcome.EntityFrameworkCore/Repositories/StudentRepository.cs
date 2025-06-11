using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSA.EduOutcome.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PSA.EduOutcome.Students
{
    public class StudentRepository : 
        EfCoreRepository<EduOutcomeDbContext, Student, Guid>,
        IStudentRepository
    {
        public StudentRepository(IDbContextProvider<EduOutcomeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Student> GetByStudentNumberAsync(string studentNumber)
        {
            return await (await GetQueryableAsync())
                .FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
        }

        public async Task<List<Student>> GetListByProgramAsync(Guid programId)
        {
            return await (await GetQueryableAsync())
                .Where(s => s.ProgramId == programId)
                .ToListAsync();
        }

        public async Task<Student> GetByUserIdAsync(Guid userId)
        {
            return await (await GetQueryableAsync())
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<Student> GetWithDetailsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Students
                .Include(s => s.Program)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .Include(s => s.Responses)
                    .ThenInclude(r => r.Question)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public override async Task<IQueryable<Student>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(s => s.Program)
                .Include(s => s.Enrollments)
                .Include(s => s.Responses);
        }
    }
} 