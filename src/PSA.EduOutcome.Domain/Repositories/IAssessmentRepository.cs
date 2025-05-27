using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PSA.EduOutcome.Entities;
using Volo.Abp.Domain.Repositories;

namespace PSA.EduOutcome.Domain.Repositories
{
    public interface IAssessmentRepository : IRepository<Assessment, Guid>
    {
        Task<Assessment> GetWithQuestionsAsync(Guid id);
        Task<List<Assessment>> GetByCourseAsync(Guid courseId);
        Task<List<Assessment>> GetPublishedByCourseAsync(Guid courseId);
    }
} 