using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PSA.EduOutcome.Entities
{
    public class Enrollment : FullAuditedEntity<Guid>
    {
        public Guid StudentId { get; private set; }
        public Guid CourseId { get; private set; }
        public string Status { get; private set; } // Enrolled, Completed, Withdrawn, Failed
        public DateTime EnrollmentDate { get; private set; }
        public DateTime? CompletionDate { get; private set; }
        public decimal? FinalGrade { get; private set; }
        public string LetterGrade { get; private set; }
        public decimal? GradePoints { get; private set; }

        // Navigation properties
        public virtual Student Student { get; private set; }
        public virtual Course Course { get; private set; }

        protected Enrollment()
        {
            // Required for EF Core
        }

        public Enrollment(
            Guid id,
            Guid studentId,
            Guid courseId) : base(id)
        {
            StudentId = studentId;
            CourseId = courseId;
            Status = EnrollmentStatus.Enrolled;
            EnrollmentDate = DateTime.Now;
        }

        public void UpdateStatus(string status)
        {
            if (!EnrollmentStatus.IsValid(status))
            {
                throw new BusinessException($"Invalid enrollment status: {status}");
            }

            Status = status;

            if (status == EnrollmentStatus.Completed || status == EnrollmentStatus.Failed)
            {
                CompletionDate = DateTime.Now;
            }
        }

        public void SetFinalGrade(decimal grade)
        {
            if (grade < 0 || grade > 100)
            {
                throw new BusinessException("Final grade must be between 0 and 100.");
            }

            FinalGrade = grade;
            LetterGrade = CalculateLetterGrade(grade);
            GradePoints = CalculateGradePoints(LetterGrade);

            // Update status based on grade
            Status = grade >= 60 ? EnrollmentStatus.Completed : EnrollmentStatus.Failed;
            CompletionDate = DateTime.Now;
        }

        private string CalculateLetterGrade(decimal grade)
        {
            return grade switch
            {
                >= 95 => "A+",
                >= 90 => "A",
                >= 85 => "B+",
                >= 80 => "B",
                >= 75 => "C+",
                >= 70 => "C",
                >= 65 => "D+",
                >= 60 => "D",
                _ => "F"
            };
        }

        private decimal CalculateGradePoints(string letterGrade)
        {
            return letterGrade switch
            {
                "A+" => 4.0m,
                "A" => 4.0m,
                "B+" => 3.5m,
                "B" => 3.0m,
                "C+" => 2.5m,
                "C" => 2.0m,
                "D+" => 1.5m,
                "D" => 1.0m,
                "F" => 0.0m,
                _ => 0.0m
            };
        }

        public void Withdraw()
        {
            if (Status == EnrollmentStatus.Completed)
            {
                throw new BusinessException("Cannot withdraw from a completed course.");
            }

            Status = EnrollmentStatus.Withdrawn;
            CompletionDate = DateTime.Now;
        }

        public bool IsActive()
        {
            return Status == EnrollmentStatus.Enrolled;
        }

        public bool IsCompleted()
        {
            return Status == EnrollmentStatus.Completed || Status == EnrollmentStatus.Failed;
        }
    }

    public static class EnrollmentStatus
    {
        public const string Enrolled = "Enrolled";
        public const string Completed = "Completed";
        public const string Withdrawn = "Withdrawn";
        public const string Failed = "Failed";

        public static bool IsValid(string status)
        {
            return status == Enrolled || status == Completed || status == Withdrawn || status == Failed;
        }
    }
} 