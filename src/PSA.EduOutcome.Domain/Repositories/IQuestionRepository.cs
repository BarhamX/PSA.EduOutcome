using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PSA.EduOutcome.Entities;
using Volo.Abp.Domain.Repositories;

namespace PSA.EduOutcome.Domain.Repositories
{
    public interface IQuestionRepository : IRepository<Question, Guid>
    {
        Task<Question> GetWithMappingsAsync(Guid id);
        Task<List<Question>> GetByAssessmentAsync(Guid assessmentId);
    }
} 