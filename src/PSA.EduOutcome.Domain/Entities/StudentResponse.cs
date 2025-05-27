using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PSA.EduOutcome.Entities
{
    public class StudentResponse : FullAuditedEntity<Guid>
    {
        public Guid StudentId { get; private set; }
        public Guid QuestionId { get; private set; }
        public Guid EnrollmentId { get; private set; }
        public string ResponseText { get; private set; } // Student's answer
        public decimal AchievedMark { get; private set; }
        public DateTime? SubmittedAt { get; private set; }
        public string GraderComments { get; private set; }
        public Guid? GradedBy { get; private set; } // Instructor who graded
        public DateTime? GradedAt { get; private set; }

        // Navigation properties
        public virtual Student Student { get; private set; }
        public virtual Question Question { get; private set; }
        public virtual Enrollment Enrollment { get; private set; }

        protected StudentResponse()
        {
            // Required for EF Core
        }

        public StudentResponse(
            Guid id,
            Guid studentId,
            Guid questionId,
            Guid enrollmentId,
            string responseText = null) : base(id)
        {
            StudentId = studentId;
            QuestionId = questionId;
            EnrollmentId = enrollmentId;
            ResponseText = responseText;
            AchievedMark = 0;
            SubmittedAt = DateTime.Now;
        }

        public void UpdateResponse(string responseText)
        {
            ResponseText = responseText;
            SubmittedAt = DateTime.Now;
            // Reset grade when response is updated
            AchievedMark = 0;
            GraderComments = null;
            GradedBy = null;
            GradedAt = null;
        }

        public void SetGrade(decimal achievedMark, string comments, Guid graderId)
        {
            if (achievedMark < 0)
            {
                throw new BusinessException("Achieved mark cannot be negative.");
            }

            // Note: We don't validate against Question.MaxMark here as the entity might not be loaded
            // This validation should be done at the application service level
            AchievedMark = achievedMark;
            GraderComments = comments;
            GradedBy = graderId;
            GradedAt = DateTime.Now;
        }

        public void UpdateGrade(decimal achievedMark, string comments)
        {
            if (achievedMark < 0)
            {
                throw new BusinessException("Achieved mark cannot be negative.");
            }

            AchievedMark = achievedMark;
            GraderComments = comments;
            GradedAt = DateTime.Now;
        }

        public bool IsGraded()
        {
            return GradedAt.HasValue;
        }

        public decimal GetPercentageScore(decimal maxMark)
        {
            if (maxMark <= 0)
            {
                return 0;
            }
            return (AchievedMark / maxMark) * 100;
        }
    }
} 