using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using PSA.EduOutcome.Domain.Events;

namespace PSA.EduOutcome.Entities
{
    public class Course : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public int Credits { get; private set; }
        public int Semester { get; private set; } // 1, 2, 3 (Fall, Spring, Summer)
        public int Year { get; private set; }
        public Guid ProgramId { get; private set; }
        public Guid FacultyId { get; private set; }
        public Guid? InstructorId { get; private set; } // Reference to Identity User
        public string Status { get; private set; } // Active, Inactive, Archived
        public bool IsElective { get; private set; }
        public int AcademicYear { get; private set; }
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
            int semester,
            int year,
            Guid programId,
            Guid facultyId,
            string description = null) : base(id)
        {
            SetName(name);
            SetCode(code);
            SetCredits(credits);
            SetSemester(semester);
            SetYear(year);
            ProgramId = programId;
            FacultyId = facultyId;
            Description = description;
            Status = "Active";
            IsElective = false;
            AcademicYear = 1;
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

        public void SetSemester(int semester)
        {
            if (semester <= 0)
            {
                throw new BusinessException("Semester must be greater than zero.");
            }
            Semester = semester;
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
            // Domain events would be added here in a more advanced implementation
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
            Status = "Inactive";
        }

        public void Archive()
        {
            IsActive = false;
            Status = "Archived";
        }

        public void SetStatus(string status)
        {
            Status = Check.NotNullOrWhiteSpace(status, nameof(status), maxLength: 50);
        }

        public void SetIsElective(bool isElective)
        {
            IsElective = isElective;
        }

        public void SetAcademicYear(int academicYear)
        {
            if (academicYear <= 0)
            {
                throw new BusinessException("Academic year must be greater than zero.");
            }
            AcademicYear = academicYear;
        }
    }
}
