using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PSA.EduOutcome.Entities
{
    public class Program : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public string Degree { get; private set; } // Bachelor, Master, PhD, etc.
        public int DurationInSemesters { get; private set; }
        public int TotalCredits { get; private set; }
        public Guid FacultyId { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation properties
        public virtual Faculty Faculty { get; private set; }
        public virtual ICollection<Course> Courses { get; private set; }

        protected Program()
        {
            // Required for EF Core
            Courses = new HashSet<Course>();
        }

        public Program(
            Guid id,
            string name,
            string code,
            string degree,
            int durationInSemesters,
            int totalCredits,
            Guid facultyId,
            string description = null) : base(id)
        {
            SetName(name);
            SetCode(code);
            SetDegree(degree);
            SetDuration(durationInSemesters);
            SetTotalCredits(totalCredits);
            FacultyId = facultyId;
            Description = description;
            IsActive = true;
            Courses = new HashSet<Course>();
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 200);
        }

        public void SetCode(string code)
        {
            Code = Check.NotNullOrWhiteSpace(code, nameof(code), maxLength: 50);
        }

        public void SetDegree(string degree)
        {
            Degree = Check.NotNullOrWhiteSpace(degree, nameof(degree), maxLength: 100);
        }

        public void SetDuration(int durationInSemesters)
        {
            if (durationInSemesters <= 0)
            {
                throw new BusinessException("Program duration must be greater than zero.");
            }
            DurationInSemesters = durationInSemesters;
        }

        public void SetTotalCredits(int totalCredits)
        {
            if (totalCredits <= 0)
            {
                throw new BusinessException("Total credits must be greater than zero.");
            }
            TotalCredits = totalCredits;
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
