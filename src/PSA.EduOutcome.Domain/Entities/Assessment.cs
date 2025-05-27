using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PSA.EduOutcome.Entities
{
    public class Assessment : FullAuditedAggregateRoot<Guid>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Type { get; private set; } // Exam, Quiz, Assignment, Project, etc.
        public decimal TotalMarks { get; private set; }
        public decimal Weight { get; private set; } // Percentage of final grade
        public Guid CourseId { get; private set; }
        public DateTime DueDate { get; private set; }
        public bool IsPublished { get; private set; }
        public bool AllowLateSubmission { get; private set; }
        public int? LatePenaltyPercentage { get; private set; }

        // Navigation properties
        public virtual Course Course { get; private set; }
        public virtual ICollection<Question> Questions { get; private set; }

        protected Assessment()
        {
            // Required for EF Core
            Questions = new HashSet<Question>();
        }

        public Assessment(
            Guid id,
            string title,
            string type,
            decimal totalMarks,
            decimal weight,
            Guid courseId,
            DateTime dueDate,
            string description = null) : base(id)
        {
            SetTitle(title);
            SetType(type);
            SetTotalMarks(totalMarks);
            SetWeight(weight);
            CourseId = courseId;
            SetDueDate(dueDate);
            Description = description;
            IsPublished = false;
            AllowLateSubmission = false;
            Questions = new HashSet<Question>();
        }

        public void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 200);
        }

        public void SetType(string type)
        {
            if (!AssessmentType.IsValid(type))
            {
                throw new BusinessException($"Invalid assessment type: {type}");
            }
            Type = type;
        }

        public void SetTotalMarks(decimal totalMarks)
        {
            if (totalMarks <= 0)
            {
                throw new BusinessException("Total marks must be greater than zero.");
            }
            TotalMarks = totalMarks;
        }

        public void SetWeight(decimal weight)
        {
            if (weight <= 0 || weight > 100)
            {
                throw new BusinessException("Assessment weight must be between 0 and 100.");
            }
            Weight = weight;
        }

        public void SetDueDate(DateTime dueDate)
        {
            if (dueDate < DateTime.Now)
            {
                throw new BusinessException("Due date cannot be in the past.");
            }
            DueDate = dueDate;
        }

        public void ConfigureLateSubmission(bool allow, int? penaltyPercentage = null)
        {
            AllowLateSubmission = allow;
            if (allow && penaltyPercentage.HasValue)
            {
                if (penaltyPercentage < 0 || penaltyPercentage > 100)
                {
                    throw new BusinessException("Late penalty percentage must be between 0 and 100.");
                }
                LatePenaltyPercentage = penaltyPercentage;
            }
            else
            {
                LatePenaltyPercentage = null;
            }
        }

        public void AddQuestion(Question question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            var currentTotal = Questions.Sum(q => q.MaxMark);
            if (currentTotal + question.MaxMark > TotalMarks)
            {
                throw new BusinessException($"Adding this question would exceed the assessment's total marks. Current: {currentTotal}, Trying to add: {question.MaxMark}, Max allowed: {TotalMarks}");
            }

            Questions.Add(question);
        }

        public void RemoveQuestion(Guid questionId)
        {
            var question = Questions.FirstOrDefault(q => q.Id == questionId);
            if (question != null)
            {
                Questions.Remove(question);
            }
        }

        public void Publish()
        {
            ValidateAssessment();
            IsPublished = true;
        }

        public void Unpublish()
        {
            IsPublished = false;
        }

        public void ValidateAssessment()
        {
            var totalQuestionMarks = Questions.Sum(q => q.MaxMark);
            if (Math.Abs(totalQuestionMarks - TotalMarks) > 0.01m)
            {
                throw new BusinessException($"Total question marks ({totalQuestionMarks}) does not match assessment total marks ({TotalMarks}).");
            }

            // Validate that all questions have LO mappings
            foreach (var question in Questions)
            {
                if (!question.QuestionLearningOutcomes.Any())
                {
                    throw new BusinessException($"Question '{question.Text}' is not mapped to any learning outcomes.");
                }
            }
        }

        public bool IsOverdue()
        {
            return DateTime.Now > DueDate;
        }

        public decimal CalculateFinalMark(decimal achievedMark, DateTime? submissionDate = null)
        {
            if (achievedMark < 0 || achievedMark > TotalMarks)
            {
                throw new BusinessException($"Achieved mark must be between 0 and {TotalMarks}.");
            }

            var finalMark = achievedMark;

            // Apply late penalty if applicable
            if (submissionDate.HasValue && submissionDate > DueDate && LatePenaltyPercentage.HasValue)
            {
                var penalty = achievedMark * (LatePenaltyPercentage.Value / 100m);
                finalMark = achievedMark - penalty;
            }

            return Math.Max(0, finalMark); // Ensure mark doesn't go below 0
        }
    }

    public static class AssessmentType
    {
        public const string Exam = "Exam";
        public const string Quiz = "Quiz";
        public const string Assignment = "Assignment";
        public const string Project = "Project";
        public const string Lab = "Lab";
        public const string Presentation = "Presentation";
        public const string Other = "Other";

        public static bool IsValid(string type)
        {
            return type == Exam || type == Quiz || type == Assignment || 
                   type == Project || type == Lab || type == Presentation || type == Other;
        }
    }
}
