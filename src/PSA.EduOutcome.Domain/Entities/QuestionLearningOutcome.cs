using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PSA.EduOutcome.Entities
{
    /// <summary>
    /// Many-to-many relationship between Questions and Learning Outcomes
    /// Tracks how marks are distributed across LOs for each question
    /// </summary>
    public class QuestionLearningOutcome : CreationAuditedEntity<Guid>
    {
        public Guid QuestionId { get; private set; }
        public Guid LearningOutcomeId { get; private set; }
        public decimal AllocatedMark { get; private set; }
        public decimal Percentage { get; private set; } // Percentage of question mark allocated to this LO

        // Navigation properties
        public virtual Question Question { get; private set; }
        public virtual LearningOutcome LearningOutcome { get; private set; }

        protected QuestionLearningOutcome()
        {
            // Required for EF Core
        }

        public QuestionLearningOutcome(
            Guid id,
            Guid questionId,
            Guid learningOutcomeId,
            decimal allocatedMark,
            decimal percentage) : base(id)
        {
            QuestionId = questionId;
            LearningOutcomeId = learningOutcomeId;
            SetAllocatedMark(allocatedMark);
            SetPercentage(percentage);
        }

        public void SetAllocatedMark(decimal mark)
        {
            if (mark < 0)
            {
                throw new BusinessException("Allocated mark cannot be negative.");
            }
            AllocatedMark = mark;
        }

        public void SetPercentage(decimal percentage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new BusinessException("Percentage must be between 0 and 100.");
            }
            Percentage = percentage;
        }

        public void UpdateAllocation(decimal allocatedMark, decimal percentage)
        {
            SetAllocatedMark(allocatedMark);
            SetPercentage(percentage);
        }
    }
} 