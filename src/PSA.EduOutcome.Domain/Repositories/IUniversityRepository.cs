using System;
using System.Threading.Tasks;
using PSA.EduOutcome.Entities;
using Volo.Abp.Domain.Repositories;

namespace PSA.EduOutcome.Domain.Repositories
{
    public interface IUniversityRepository : IRepository<University, Guid>
    {
        Task<University> FindByCodeAsync(string code);
    }
} 