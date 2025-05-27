using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;
using PSA.EduOutcome.Entities;

namespace PSA.EduOutcome.Entities
{
    public class LearningOutcome : FullAuditedAggregateRoot<Guid>
    {
        public string Code { get; private set; } // e.g., "LO1", "LO2"
        public string Description { get; private set; }
        public decimal MaxMark { get; private set; }
        public Guid CourseId { get; private set; }
        public string Category { get; private set; } // Knowledge, Skills, Competence
        public int DisplayOrder { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation properties
        public virtual Course Course { get; private set; }
        public virtual ICollection<QuestionLearningOutcome> QuestionMappings { get; private set; }

        protected LearningOutcome()
        {
            // Required for EF Core
            QuestionMappings = new HashSet<QuestionLearningOutcome>();
        }

        public LearningOutcome(
            Guid id,
            string code,
            string description,
            decimal maxMark,
            Guid courseId,
            string category,
            int displayOrder = 1) : base(id)
        {
            SetCode(code);
            SetDescription(description);
            SetMaxMark(maxMark);
            CourseId = courseId;
            SetCategory(category);
            DisplayOrder = displayOrder;
            IsActive = true;
            QuestionMappings = new HashSet<QuestionLearningOutcome>();
        }

        public void SetCode(string code)
        {
            Code = Check.NotNullOrWhiteSpace(code, nameof(code), maxLength: 20);
        }

        public void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 1000);
        }

        public void SetMaxMark(decimal maxMark)
        {
            if (maxMark <= 0)
            {
                throw new BusinessException("Max mark must be greater than zero.");
            }
            if (maxMark > 100)
            {
                throw new BusinessException("Max mark cannot exceed 100.");
            }
            MaxMark = maxMark;
        }

        public void SetCategory(string category)
        {
            if (!LearningOutcomeCategory.IsValid(category))
            {
                throw new BusinessException($"Invalid learning outcome category: {category}");
            }
            Category = category;
        }

        public void SetDisplayOrder(int order)
        {
            if (order < 1)
            {
                throw new BusinessException("Display order must be at least 1.");
            }
            DisplayOrder = order;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public decimal CalculateStudentAchievement(IEnumerable<StudentResponse> studentResponses)
        {
            decimal totalAchieved = 0;
            decimal totalPossible = 0;

            foreach (var mapping in QuestionMappings)
            {
                var response = studentResponses.FirstOrDefault(r => r.QuestionId == mapping.QuestionId);
                if (response != null)
                {
                    totalAchieved += response.AchievedMark * (mapping.Percentage / 100m);
                    totalPossible += mapping.AllocatedMark;
                }
            }

            return totalPossible > 0 ? (totalAchieved / totalPossible) * MaxMark : 0;
        }
    }

    public static class LearningOutcomeCategory
    {
        public const string Knowledge = "Knowledge";
        public const string Skills = "Skills";
        public const string Competence = "Competence";

        public static bool IsValid(string category)
        {
            return category == Knowledge || category == Skills || category == Competence;
        }
    }
}
