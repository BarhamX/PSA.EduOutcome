using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSA.EduOutcome.Domain.Repositories;
using PSA.EduOutcome.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PSA.EduOutcome.EntityFrameworkCore.Repositories
{
    public class QuestionRepository : EfCoreRepository<EduOutcomeDbContext, Question, Guid>, IQuestionRepository
    {
        public QuestionRepository(IDbContextProvider<EduOutcomeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Question> GetWithMappingsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();

            return await dbContext.Questions
                .Include(q => q.Assessment)
                .Include(q => q.QuestionLearningOutcomes)
                    .ThenInclude(mapping => mapping.LearningOutcome)
                .FirstAsync(q => q.Id == id);
        }

        public async Task<List<Question>> GetByAssessmentAsync(Guid assessmentId)
        {
            var dbContext = await GetDbContextAsync();

            return await dbContext.Questions
                .Where(q => q.AssessmentId == assessmentId)
                .ToListAsync();
        }
    }
}
