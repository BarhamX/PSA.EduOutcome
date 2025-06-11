using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace PSA.EduOutcome.DTOs
{
    public class StudentResponseDto : AuditedEntityDto<Guid>
    {
        public Guid StudentId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid EnrollmentId { get; set; }
        public string ResponseText { get; set; }
        public decimal AchievedMark { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public string GraderComments { get; set; }
        public Guid? GradedBy { get; set; }
        public DateTime? GradedAt { get; set; }
        public string StudentName { get; set; }
        public string QuestionText { get; set; }
        public decimal QuestionMaxMark { get; set; }
        public string GraderName { get; set; }
    }

    public class CreateUpdateStudentResponseDto
    {
        public Guid StudentId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid EnrollmentId { get; set; }
        public string ResponseText { get; set; }
    }

    public class GradeStudentResponseDto
    {
        public decimal AchievedMark { get; set; }
        public string GraderComments { get; set; }
    }

    public class BulkGradeDto
    {
        public Guid AssessmentId { get; set; }
        public List<StudentGradeEntryDto> Grades { get; set; }
    }

    public class StudentGradeEntryDto
    {
        public Guid StudentId { get; set; }
        public Guid QuestionId { get; set; }
        public decimal AchievedMark { get; set; }
        public string Comments { get; set; }
    }
} 