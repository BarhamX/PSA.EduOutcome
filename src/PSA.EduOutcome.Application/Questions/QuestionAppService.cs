using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PSA.EduOutcome.Domain.Repositories;
using PSA.EduOutcome.DTOs;
using PSA.EduOutcome.Entities;
using PSA.EduOutcome.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace PSA.EduOutcome.Questions
{
    [Authorize(EduOutcomePermissions.Questions.Default)]
    public class QuestionAppService : ApplicationService, IQuestionAppService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IRepository<Assessment, Guid> _assessmentRepository;
        private readonly IRepository<StudentResponse, Guid> _studentResponseRepository;

        public QuestionAppService(
            IQuestionRepository questionRepository,
            IRepository<Assessment, Guid> assessmentRepository,
            IRepository<StudentResponse, Guid> studentResponseRepository)
        {
            _questionRepository = questionRepository;
            _assessmentRepository = assessmentRepository;
            _studentResponseRepository = studentResponseRepository;
        }

        public async Task<QuestionDto> GetAsync(Guid id)
        {
            var question = await _questionRepository.GetWithMappingsAsync(id);
            return MapToDto(question);
        }

        public async Task<PagedResultDto<QuestionDto>> GetListAsync(GetQuestionListDto input)
        {
            var queryable = await _questionRepository.WithDetailsAsync(q => q.Assessment, q => q.QuestionLearningOutcomes);

            queryable = queryable
                .WhereIf(input.AssessmentId.HasValue, q => q.AssessmentId == input.AssessmentId.Value)
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), q => q.Text.Contains(input.Filter) || q.Description.Contains(input.Filter))
                .WhereIf(!input.QuestionType.IsNullOrWhiteSpace(), q => q.QuestionType == input.QuestionType)
                .WhereIf(input.HasLearningOutcomeMappings.HasValue,
                    q => input.HasLearningOutcomeMappings.Value
                        ? q.QuestionLearningOutcomes.Any()
                        : !q.QuestionLearningOutcomes.Any());

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var sorting = string.IsNullOrWhiteSpace(input.Sorting)
                ? nameof(Question.QuestionNumber)
                : input.Sorting;

            var questions = await AsyncExecuter.ToListAsync(
                queryable
                    .OrderBy(sorting)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount));

            var items = questions.Select(MapToDto).ToList();
            return new PagedResultDto<QuestionDto>(totalCount, items);
        }

        [Authorize(EduOutcomePermissions.Questions.Create)]
        public async Task<QuestionDto> CreateAsync(CreateUpdateQuestionDto input)
        {
            await EnsureAssessmentEditableAsync(input.AssessmentId);

            await EnsureUniqueQuestionNumberAsync(input.AssessmentId, input.QuestionNumber);
            await EnsureQuestionTotalsWithinAssessmentAsync(input.AssessmentId, input.MaxMark);

            var question = new Question(
                GuidGenerator.Create(),
                input.Text,
                input.QuestionNumber,
                input.MaxMark,
                input.AssessmentId,
                input.QuestionType,
                input.Description);

            await _questionRepository.InsertAsync(question, autoSave: true);

            return await GetAsync(question.Id);
        }

        [Authorize(EduOutcomePermissions.Questions.Edit)]
        public async Task<QuestionDto> UpdateAsync(Guid id, CreateUpdateQuestionDto input)
        {
            var question = await _questionRepository.GetWithMappingsAsync(id);

            if (question.AssessmentId != input.AssessmentId)
            {
                throw new BusinessException("Changing the assessment of a question is not supported.");
            }

            await EnsureAssessmentEditableAsync(question.AssessmentId);

            await EnsureUniqueQuestionNumberAsync(question.AssessmentId, input.QuestionNumber, id);
            await EnsureQuestionTotalsWithinAssessmentAsync(question.AssessmentId, input.MaxMark, id);

            question.SetText(input.Text);
            question.SetQuestionNumber(input.QuestionNumber);
            question.SetMaxMark(input.MaxMark);
            question.SetQuestionType(input.QuestionType);
            question.SetDescription(input.Description);

            await _questionRepository.UpdateAsync(question, autoSave: true);

            return MapToDto(question);
        }

        [Authorize(EduOutcomePermissions.Questions.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var question = await _questionRepository.GetWithMappingsAsync(id);

            await EnsureAssessmentEditableAsync(question.AssessmentId);

            if (question.QuestionLearningOutcomes.Any())
            {
                throw new BusinessException("Cannot delete a question that has learning outcome mappings. Remove the mappings first.");
            }

            var hasResponses = await _studentResponseRepository.AnyAsync(sr => sr.QuestionId == id);
            if (hasResponses)
            {
                throw new BusinessException("Cannot delete a question that already has student responses recorded.");
            }

            await _questionRepository.DeleteAsync(id, autoSave: true);
        }

        private async Task EnsureAssessmentEditableAsync(Guid assessmentId)
        {
            var assessment = await _assessmentRepository.GetAsync(assessmentId);
            if (assessment.IsPublished)
            {
                throw new BusinessException("Cannot modify questions for a published assessment.");
            }
        }

        private async Task EnsureUniqueQuestionNumberAsync(Guid assessmentId, int questionNumber, Guid? excludeQuestionId = null)
        {
            var exists = await _questionRepository.AnyAsync(q =>
                q.AssessmentId == assessmentId &&
                q.QuestionNumber == questionNumber &&
                (!excludeQuestionId.HasValue || q.Id != excludeQuestionId.Value));

            if (exists)
            {
                throw new BusinessException($"Question number {questionNumber} already exists for this assessment.");
            }
        }

        private async Task EnsureQuestionTotalsWithinAssessmentAsync(Guid assessmentId, decimal candidateMaxMark, Guid? excludeQuestionId = null)
        {
            var assessment = await _assessmentRepository.GetAsync(assessmentId);
            var questions = await _questionRepository.GetListAsync(q => q.AssessmentId == assessmentId);

            var existingTotal = questions
                .Where(q => !excludeQuestionId.HasValue || q.Id != excludeQuestionId.Value)
                .Sum(q => q.MaxMark);

            if (existingTotal + candidateMaxMark > assessment.TotalMarks + 0.01m)
            {
                throw new BusinessException($"Adding or updating this question would exceed the assessment total marks ({assessment.TotalMarks}). Current allocated: {existingTotal}.");
            }
        }

        private QuestionDto MapToDto(Question question)
        {
            var dto = ObjectMapper.Map<Question, QuestionDto>(question);
            dto.AssessmentTitle ??= question.Assessment?.Title;
            dto.LearningOutcomeMappings ??= new List<QuestionLearningOutcomeDto>();
            return dto;
        }
    }
}
