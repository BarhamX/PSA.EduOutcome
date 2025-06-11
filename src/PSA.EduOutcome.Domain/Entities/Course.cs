using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using EduOutcome.Domain.Events;

namespace PSA.EduOutcome.Entities
{
    public class Course : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public int Credits { get; private set; }
        public string Semester { get; private set; } // Fall, Spring, Summer
        public int Year { get; private set; }
        public Guid ProgramId { get; private set; }
        public Guid? InstructorId { get; private set; } // Reference to Identity User
        public bool IsActive { get; private set; }

        // Navigation properties
        public virtual Program Program { get; private set; }
        public virtual ICollection<LearningOutcome> LearningOutcomes { get; private set; }
        public virtual ICollection<Assessment> Assessments { get; private set; }
        public virtual ICollection<Enrollment> Enrollments { get; private set; }

        protected Course()
        {
            // Required for EF Core
            LearningOutcomes = new HashSet<LearningOutcome>();
            Assessments = new HashSet<Assessment>();
            Enrollments = new HashSet<Enrollment>();
        }

        public Course(
            Guid id,
            string name,
            string code,
            int credits,
            string semester,
            int year,
            Guid programId,
            string description = null) : base(id)
        {
            SetName(name);
            SetCode(code);
            SetCredits(credits);
            SetSemester(semester);
            SetYear(year);
            ProgramId = programId;
            Description = description;
            IsActive = true;
            LearningOutcomes = new HashSet<LearningOutcome>();
            Assessments = new HashSet<Assessment>();
            Enrollments = new HashSet<Enrollment>();
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 200);
        }

        public void SetCode(string code)
        {
            Code = Check.NotNullOrWhiteSpace(code, nameof(code), maxLength: 50);
        }

        public void SetCredits(int credits)
        {
            if (credits <= 0)
            {
                throw new BusinessException("Course credits must be greater than zero.");
            }
            Credits = credits;
        }

        public void SetSemester(string semester)
        {
            Semester = Check.NotNullOrWhiteSpace(semester, nameof(semester), maxLength: 20);
        }

        public void SetYear(int year)
        {
            if (year < 2000 || year > 2100)
            {
                throw new BusinessException("Invalid year.");
            }
            Year = year;
        }

        public void AssignInstructor(Guid instructorId)
        {
            InstructorId = instructorId;
        }

        public void AddLearningOutcome(LearningOutcome learningOutcome)
        {
            if (learningOutcome == null)
            {
                throw new ArgumentNullException(nameof(learningOutcome));
            }

            var currentTotalMarks = LearningOutcomes.Sum(lo => lo.MaxMark);
            if (currentTotalMarks + learningOutcome.MaxMark > 100)
            {
                throw new BusinessException($"The total marks for learning outcomes would exceed 100. Current: {currentTotalMarks}, Trying to add: {learningOutcome.MaxMark}");
            }

            LearningOutcomes.Add(learningOutcome);
            // Publish domain event for a new learning outcome added.
            AddDomainEvent(new LearningOutcomeAddedEvent(this.Id, learningOutcome.Id));
        }

        public void RemoveLearningOutcome(Guid learningOutcomeId)
        {
            var lo = LearningOutcomes.FirstOrDefault(x => x.Id == learningOutcomeId);
            if (lo != null)
            {
                LearningOutcomes.Remove(lo);
            }
        }

        public void AddAssessment(Assessment assessment)
        {
            if (assessment == null)
            {
                throw new ArgumentNullException(nameof(assessment));
            }

            Assessments.Add(assessment);
            // Optionally, publish a different domain event here if needed.
        }

        public void ValidateLearningOutcomesCoverage()
        {
            var totalMarks = LearningOutcomes.Sum(lo => lo.MaxMark);
            if (totalMarks != 100)
            {
                throw new BusinessException($"Total learning outcome marks must equal 100. Current total: {totalMarks}");
            }
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
