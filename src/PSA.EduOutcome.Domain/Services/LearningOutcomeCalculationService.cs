using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PSA.EduOutcome.Domain.Repositories;
using PSA.EduOutcome.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace PSA.EduOutcome.Domain.Services;

public class LearningOutcomeCalculationService : ILearningOutcomeCalculationService, ITransientDependency
{
    private readonly ICourseRepository _courseRepository;
    private readonly IRepository<LearningOutcome, Guid> _learningOutcomeRepository;
    private readonly IRepository<QuestionLearningOutcome, Guid> _questionLearningOutcomeRepository;
    private readonly IRepository<Question, Guid> _questionRepository;
    private readonly IRepository<StudentResponse, Guid> _studentResponseRepository;

    public LearningOutcomeCalculationService(
        ICourseRepository courseRepository,
        IRepository<LearningOutcome, Guid> learningOutcomeRepository,
        IRepository<QuestionLearningOutcome, Guid> questionLearningOutcomeRepository,
        IRepository<Question, Guid> questionRepository,
        IRepository<StudentResponse, Guid> studentResponseRepository)
    {
        _courseRepository = courseRepository;
        _learningOutcomeRepository = learningOutcomeRepository;
        _questionLearningOutcomeRepository = questionLearningOutcomeRepository;
        _questionRepository = questionRepository;
        _studentResponseRepository = studentResponseRepository;
    }

    public async Task<decimal> CalculateStudentLearningOutcomeAchievementAsync(Guid studentId, Guid learningOutcomeId, Guid courseId)
    {
        var context = await BuildContextAsync(courseId, studentId);

        if (!context.LearningOutcomes.TryGetValue(learningOutcomeId, out var learningOutcome))
        {
            return 0m;
        }

        return CalculateOutcomePercentage(learningOutcome, context);
    }

    public async Task<Dictionary<Guid, decimal>> CalculateStudentCourseOutcomesAsync(Guid studentId, Guid courseId)
    {
        var context = await BuildContextAsync(courseId, studentId);

        var result = new Dictionary<Guid, decimal>();

        foreach (var outcome in context.LearningOutcomes.Values)
        {
            result[outcome.Id] = CalculateOutcomePercentage(outcome, context);
        }

        return result;
    }

    public async Task<decimal> CalculateStudentCourseAchievementAsync(Guid studentId, Guid courseId)
    {
        var context = await BuildContextAsync(courseId, studentId);

        if (context.LearningOutcomes.Count == 0)
        {
            return 0m;
        }

        var outcomes = new Dictionary<Guid, decimal>();
        decimal weightedSum = 0m;
        decimal totalWeight = 0m;

        foreach (var outcome in context.LearningOutcomes.Values)
        {
            var percentage = CalculateOutcomePercentage(outcome, context);
            outcomes[outcome.Id] = percentage;
            weightedSum += (percentage / 100m) * outcome.MaxMark;
            totalWeight += outcome.MaxMark;
        }

        if (totalWeight <= 0m)
        {
            return 0m;
        }

        var courseAchievement = (weightedSum / totalWeight) * 100m;
        return NormalizePercentage(courseAchievement);
    }

    public async Task<LearningOutcomeValidationResult> ValidateLearningOutcomeCoverageAsync(Guid courseId)
    {
        var course = await _courseRepository.GetWithDetailsAsync(courseId);

        var result = new LearningOutcomeValidationResult
        {
            TotalCoverage = course.LearningOutcomes.Sum(lo => lo.MaxMark)
        };

        foreach (var lo in course.LearningOutcomes)
        {
            result.OutcomeCoverage[lo.Id] = lo.MaxMark;
        }

        try
        {
            course.ValidateLearningOutcomesCoverage();
            result.IsValid = true;
        }
        catch
        {
            result.IsValid = false;
            result.Errors.Add("Learning outcome marks must total 100.");
        }

        return result;
    }

    private async Task<CourseOutcomeCalculationContext> BuildContextAsync(Guid courseId, Guid studentId)
    {
        var learningOutcomes = await _learningOutcomeRepository.GetListAsync(lo => lo.CourseId == courseId);
        var learningOutcomeById = learningOutcomes.ToDictionary(lo => lo.Id);

        if (learningOutcomeById.Count == 0)
        {
            return CourseOutcomeCalculationContext.Empty;
        }

        var learningOutcomeIds = learningOutcomeById.Keys.ToList();

        var mappings = await _questionLearningOutcomeRepository.GetListAsync(qm => learningOutcomeIds.Contains(qm.LearningOutcomeId));
        var mappingsByOutcome = mappings
            .GroupBy(m => m.LearningOutcomeId)
            .ToDictionary(g => g.Key, g => g.ToList());

        if (mappingsByOutcome.Count == 0)
        {
            return new CourseOutcomeCalculationContext(learningOutcomeById, new Dictionary<Guid, List<QuestionLearningOutcome>>(), new Dictionary<Guid, Question>(), new Dictionary<Guid, StudentResponse>());
        }

        var questionIds = mappings.Select(m => m.QuestionId).Distinct().ToList();
        var questions = await _questionRepository.GetListAsync(q => questionIds.Contains(q.Id));
        var questionById = questions.ToDictionary(q => q.Id);

        var responses = await _studentResponseRepository.GetListAsync(sr => sr.StudentId == studentId && questionIds.Contains(sr.QuestionId));
        var responseByQuestion = responses.ToDictionary(sr => sr.QuestionId, sr => sr);

        return new CourseOutcomeCalculationContext(learningOutcomeById, mappingsByOutcome, questionById, responseByQuestion);
    }

    private static decimal CalculateOutcomePercentage(LearningOutcome learningOutcome, CourseOutcomeCalculationContext context)
    {
        if (!context.MappingsByOutcome.TryGetValue(learningOutcome.Id, out var mappings) || mappings.Count == 0)
        {
            return 0m;
        }

        decimal totalAllocated = 0m;
        decimal totalAchieved = 0m;

        foreach (var mapping in mappings)
        {
            totalAllocated += mapping.AllocatedMark;

            if (!context.Questions.TryGetValue(mapping.QuestionId, out var question))
            {
                continue;
            }

            if (!context.ResponsesByQuestion.TryGetValue(mapping.QuestionId, out var response))
            {
                continue;
            }

            if (question.MaxMark <= 0m)
            {
                continue;
            }

            var ratio = response.AchievedMark / question.MaxMark;

            if (ratio < 0m)
            {
                ratio = 0m;
            }
            else if (ratio > 1m)
            {
                ratio = 1m;
            }

            totalAchieved += ratio * mapping.AllocatedMark;
        }

        if (totalAllocated <= 0m)
        {
            return 0m;
        }

        var percentage = (totalAchieved / totalAllocated) * 100m;
        return NormalizePercentage(percentage);
    }

    private static decimal NormalizePercentage(decimal value)
    {
        if (value < 0m)
        {
            value = 0m;
        }
        else if (value > 100m)
        {
            value = 100m;
        }

        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }

    private sealed record CourseOutcomeCalculationContext(
        IReadOnlyDictionary<Guid, LearningOutcome> LearningOutcomes,
        IReadOnlyDictionary<Guid, List<QuestionLearningOutcome>> MappingsByOutcome,
        IReadOnlyDictionary<Guid, Question> Questions,
        IReadOnlyDictionary<Guid, StudentResponse> ResponsesByQuestion)
    {
        public static CourseOutcomeCalculationContext Empty { get; } = new(
            new Dictionary<Guid, LearningOutcome>(),
            new Dictionary<Guid, List<QuestionLearningOutcome>>(),
            new Dictionary<Guid, Question>(),
            new Dictionary<Guid, StudentResponse>());
    }
}
