using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PSA.EduOutcome.Entities;

namespace PSA.EduOutcome.Domain.Services
{
    public interface ILearningOutcomeCalculationService
    {
        /// <summary>
        /// Calculate a student's achievement for a specific learning outcome
        /// </summary>
        Task<decimal> CalculateStudentLearningOutcomeAchievementAsync(Guid studentId, Guid learningOutcomeId, Guid courseId);
        
        /// <summary>
        /// Calculate a student's achievement for all learning outcomes in a course
        /// </summary>
        Task<Dictionary<Guid, decimal>> CalculateStudentCourseOutcomesAsync(Guid studentId, Guid courseId);
        
        /// <summary>
        /// Calculate the overall course achievement for a student
        /// </summary>
        Task<decimal> CalculateStudentCourseAchievementAsync(Guid studentId, Guid courseId);
        
        /// <summary>
        /// Validate that all learning outcomes are properly covered in assessments
        /// </summary>
        Task<LearningOutcomeValidationResult> ValidateLearningOutcomeCoverageAsync(Guid courseId);
    }
    
    public class LearningOutcomeValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public Dictionary<Guid, decimal> OutcomeCoverage { get; set; } = new Dictionary<Guid, decimal>();
        public decimal TotalCoverage { get; set; }
    }
} 