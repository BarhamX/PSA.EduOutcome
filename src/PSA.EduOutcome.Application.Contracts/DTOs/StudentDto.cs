using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PSA.EduOutcome.DTOs
{
    public class StudentDto : AuditedEntityDto<Guid>
    {
        public string StudentNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public Guid ProgramId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; }
        public Guid? UserId { get; set; }
        public string ProgramName { get; set; }
    }

    public class CreateStudentDto
    {
        public string ReferenceNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public Guid ProgramId { get; set; }
    }

    public class UpdateStudentDto
    {
        public string ReferenceNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public Guid ProgramId { get; set; }
    }

    public class StudentDetailDto : StudentDto
    {
        public List<EnrollmentDto> Enrollments { get; set; }
        public Dictionary<Guid, StudentCourseOutcomeDto> CourseOutcomes { get; set; }
    }

    public class StudentCourseOutcomeDto
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public Dictionary<Guid, decimal> LearningOutcomeAchievements { get; set; }
        public decimal OverallAchievement { get; set; }
    }
}
