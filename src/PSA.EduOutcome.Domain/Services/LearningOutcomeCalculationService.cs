using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PSA.EduOutcome.Domain.Repositories;
using PSA.EduOutcome.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.DependencyInjection;

namespace PSA.EduOutcome.Domain.Services;

public class LearningOutcomeCalculationService : ILearningOutcomeCalculationService, ITransientDependency
{
    private readonly ICourseRepository _courseRepository;

    public LearningOutcomeCalculationService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<decimal> CalculateStudentLearningOutcomeAchievementAsync(Guid studentId, Guid learningOutcomeId, Guid courseId)
    {
        // Placeholder implementation since student response repositories are not available
        await Task.CompletedTask;
        return 0m;
    }

    public async Task<Dictionary<Guid, decimal>> CalculateStudentCourseOutcomesAsync(Guid studentId, Guid courseId)
    {
        await Task.CompletedTask;
        return new Dictionary<Guid, decimal>();
    }

    public async Task<decimal> CalculateStudentCourseAchievementAsync(Guid studentId, Guid courseId)
    {
        await Task.CompletedTask;
        return 0m;
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
}
