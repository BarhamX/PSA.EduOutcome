using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PSA.EduOutcome.DTOs
{
    public class AssessmentDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal Weight { get; set; }
        public Guid CourseId { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPublished { get; set; }
        public bool AllowLateSubmission { get; set; }
        public int? LatePenaltyPercentage { get; set; }
        public string CourseName { get; set; }
        public int QuestionCount { get; set; }
    }

    public class CreateUpdateAssessmentDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal Weight { get; set; }
        public Guid CourseId { get; set; }
        public DateTime DueDate { get; set; }
        public bool AllowLateSubmission { get; set; }
        public int? LatePenaltyPercentage { get; set; }
    }

    public class AssessmentDetailDto : AssessmentDto
    {
        public List<QuestionDto> Questions { get; set; }
        public decimal TotalAllocatedMarks { get; set; }
        public bool IsValid { get; set; }
        public List<string> ValidationErrors { get; set; }
    }
}
