using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PSA.EduOutcome.Entities
{
    public class Question : FullAuditedEntity<Guid>
    {
        public string Text { get; private set; }
        public string Description { get; private set; }
        public int QuestionNumber { get; private set; }
        public decimal MaxMark { get; private set; }
        public Guid AssessmentId { get; private set; }
        public string QuestionType { get; private set; } // MCQ, Essay, ShortAnswer, TrueFalse, etc.

        // Navigation properties
        public virtual Assessment Assessment { get; private set; }
        public virtual ICollection<QuestionLearningOutcome> QuestionLearningOutcomes { get; private set; }
        public virtual ICollection<StudentResponse> StudentResponses { get; private set; }

        protected Question()
        {
            // Required for EF Core
            QuestionLearningOutcomes = new HashSet<QuestionLearningOutcome>();
            StudentResponses = new HashSet<StudentResponse>();
        }

        public Question(
            Guid id,
            string text,
            int questionNumber,
            decimal maxMark,
            Guid assessmentId,
            string questionType,
            string description = null) : base(id)
        {
            SetText(text);
            SetQuestionNumber(questionNumber);
            SetMaxMark(maxMark);
            AssessmentId = assessmentId;
            SetQuestionType(questionType);
            Description = description;
            QuestionLearningOutcomes = new HashSet<QuestionLearningOutcome>();
            StudentResponses = new HashSet<StudentResponse>();
        }

        public void SetText(string text)
        {
            Text = Check.NotNullOrWhiteSpace(text, nameof(text), maxLength: 2000);
        }

        public void SetQuestionNumber(int number)
        {
            if (number <= 0)
            {
                throw new BusinessException("Question number must be greater than zero.");
            }
            QuestionNumber = number;
        }

        public void SetMaxMark(decimal maxMark)
        {
            if (maxMark <= 0)
            {
                throw new BusinessException("Max mark must be greater than zero.");
            }
            MaxMark = maxMark;
        }

        public void SetQuestionType(string type)
        {
            if (!QuestionType.IsValid(type))
            {
                throw new BusinessException($"Invalid question type: {type}");
            }
            QuestionType = type;
        }

        public void MapToLearningOutcome(Guid learningOutcomeId, decimal allocatedMark, decimal percentage)
        {
            if (QuestionLearningOutcomes.Any(qlo => qlo.LearningOutcomeId == learningOutcomeId))
            {
                throw new BusinessException($"This question is already mapped to learning outcome {learningOutcomeId}.");
            }

            var totalAllocated = QuestionLearningOutcomes.Sum(qlo => qlo.AllocatedMark) + allocatedMark;
            if (totalAllocated > MaxMark)
            {
                throw new BusinessException($"Total allocated marks ({totalAllocated}) exceeds question max mark ({MaxMark}).");
            }

            var totalPercentage = QuestionLearningOutcomes.Sum(qlo => qlo.Percentage) + percentage;
            if (totalPercentage > 100)
            {
                throw new BusinessException($"Total percentage ({totalPercentage}) exceeds 100%.");
            }
        }

        public void ValidateLearningOutcomeMappings()
        {
            var totalAllocated = QuestionLearningOutcomes.Sum(qlo => qlo.AllocatedMark);
            var totalPercentage = QuestionLearningOutcomes.Sum(qlo => qlo.Percentage);

            if (Math.Abs(totalAllocated - MaxMark) > 0.01m)
            {
                throw new BusinessException($"Total allocated marks ({totalAllocated}) does not match question max mark ({MaxMark}).");
            }

            if (Math.Abs(totalPercentage - 100) > 0.01m)
            {
                throw new BusinessException($"Total percentage ({totalPercentage}) does not equal 100%.");
            }
        }
    }

    public static class QuestionType
    {
        public const string MultipleChoice = "MultipleChoice";
        public const string Essay = "Essay";
        public const string ShortAnswer = "ShortAnswer";
        public const string TrueFalse = "TrueFalse";
        public const string Numerical = "Numerical";
        public const string Matching = "Matching";
        public const string FillInTheBlank = "FillInTheBlank";

        public static bool IsValid(string type)
        {
            return type == MultipleChoice || type == Essay || type == ShortAnswer ||
                   type == TrueFalse || type == Numerical || type == Matching || type == FillInTheBlank;
        }
    }
}
