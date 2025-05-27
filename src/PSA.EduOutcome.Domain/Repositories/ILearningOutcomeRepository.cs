using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PSA.EduOutcome.Entities;
using Volo.Abp.Domain.Repositories;

namespace PSA.EduOutcome.Domain.Repositories
{
    public interface ILearningOutcomeRepository : IRepository<LearningOutcome, Guid>
    {
        Task<List<LearningOutcome>> GetByCourseAsync(Guid courseId);
        Task<LearningOutcome> GetWithMappingsAsync(Guid id);
    }
} 