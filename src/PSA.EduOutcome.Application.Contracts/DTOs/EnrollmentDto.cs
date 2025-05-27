using System;
using Volo.Abp.Application.Dtos;

namespace PSA.EduOutcome.DTOs
{
    public class EnrollmentDto : AuditedEntityDto<Guid>
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public string Status { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public decimal? FinalGrade { get; set; }
        public string LetterGrade { get; set; }
        public decimal? GradePoints { get; set; }
        public string StudentName { get; set; }
        public string StudentNumber { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
    }

    public class CreateEnrollmentDto
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
    }

    public class UpdateEnrollmentGradeDto
    {
        public decimal FinalGrade { get; set; }
    }

    public class EnrollmentDetailDto : EnrollmentDto
    {
        public StudentDto Student { get; set; }
        public CourseDto Course { get; set; }
        public Dictionary<Guid, decimal> LearningOutcomeAchievements { get; set; }
    }
} 