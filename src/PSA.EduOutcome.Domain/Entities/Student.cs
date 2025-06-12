using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace PSA.EduOutcome.Entities
{
    public class Student : FullAuditedAggregateRoot<Guid>
    {
        public string StudentNumber { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Gender { get; private set; }
        public Guid ProgramId { get; private set; }
        public DateTime EnrollmentDate { get; private set; }
        public string Status { get; private set; } // Active, Graduated, Suspended, Withdrawn
        public Guid? UserId { get; private set; } // Link to Identity User
        public string ReferenceNumber { get; private set; }

        // Navigation properties
        public virtual Program Program { get; private set; }
        public virtual ICollection<Enrollment> Enrollments { get; private set; }
        public virtual ICollection<StudentResponse> Responses { get; private set; }

        protected Student()
        {
            // Required for EF Core
            Enrollments = new HashSet<Enrollment>();
            Responses = new HashSet<StudentResponse>();
        }

        public static Student Create(
            string firstName,
            string lastName,
            string email,
            DateTime dateOfBirth,
            string gender,
            Guid programId,
            string referenceNumber,
            string phone = null)
        {
            var studentNumber = GenerateStudentNumber();
            return new Student(
                Guid.NewGuid(),
                studentNumber,
                firstName,
                lastName,
                email,
                dateOfBirth,
                gender,
                programId,
                phone,
                referenceNumber
            );
        }

        private static string GenerateStudentNumber()
        {
            return $"STU-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        public Student(
            Guid id,
            string studentNumber,
            string firstName,
            string lastName,
            string email,
            DateTime dateOfBirth,
            string gender,
            Guid programId,
            string phone,
            string referenceNumber) : base(id)
        {
            SetStudentNumber(studentNumber);
            SetName(firstName, lastName);
            SetEmail(email);
            SetDateOfBirth(dateOfBirth);
            SetGender(gender);
            ProgramId = programId;
            Phone = phone;
            EnrollmentDate = DateTime.Now;
            Status = StudentStatus.Active;
            Enrollments = new HashSet<Enrollment>();
            Responses = new HashSet<StudentResponse>();
            ReferenceNumber = referenceNumber;
        }

        public void SetStudentNumber(string studentNumber)
        {
            StudentNumber = Check.NotNullOrWhiteSpace(studentNumber, nameof(studentNumber), maxLength: 50);
        }

        public void SetName(string firstName, string lastName)
        {
            FirstName = Check.NotNullOrWhiteSpace(firstName, nameof(firstName), maxLength: 100);
            LastName = Check.NotNullOrWhiteSpace(lastName, nameof(lastName), maxLength: 100);
        }

        public void SetEmail(string email)
        {
            Email = Check.NotNullOrWhiteSpace(email, nameof(email), maxLength: 256);
            // Add email validation if needed
        }

        public void SetDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth > DateTime.Now.AddYears(-16))
            {
                throw new BusinessException("Student must be at least 16 years old.");
            }
            DateOfBirth = dateOfBirth;
        }

        public void SetGender(string gender)
        {
            Gender = Check.NotNullOrWhiteSpace(gender, nameof(gender), maxLength: 10);
        }

        public void LinkToUser(Guid userId)
        {
            UserId = userId;
        }

        public void ChangeProgram(Guid newProgramId)
        {
            ProgramId = newProgramId;
        }

        public void UpdateStatus(string status)
        {
            if (!StudentStatus.IsValid(status))
            {
                throw new BusinessException($"Invalid student status: {status}");
            }
            Status = status;
        }

        public void Graduate()
        {
            Status = StudentStatus.Graduated;
        }

        public void Suspend()
        {
            Status = StudentStatus.Suspended;
        }

        public void Withdraw()
        {
            Status = StudentStatus.Withdrawn;
        }

        public void Reactivate()
        {
            if (Status == StudentStatus.Graduated)
            {
                throw new BusinessException("Cannot reactivate a graduated student.");
            }
            Status = StudentStatus.Active;
        }

        public void Update(
            string firstName,
            string lastName,
            string email,
            string phone,
            DateTime dateOfBirth,
            string gender,
            Guid programId)
        {
            SetName(firstName, lastName);
            SetEmail(email);
            SetDateOfBirth(dateOfBirth);
            SetGender(gender);
            ChangeProgram(programId);
            Phone = phone;
        }
    }

    public static class StudentStatus
    {
        public const string Active = "Active";
        public const string Graduated = "Graduated";
        public const string Suspended = "Suspended";
        public const string Withdrawn = "Withdrawn";

        public static bool IsValid(string status)
        {
            return status == Active || status == Graduated || status == Suspended || status == Withdrawn;
        }
    }
}
