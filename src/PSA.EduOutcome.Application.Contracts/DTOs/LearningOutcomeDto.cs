using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PSA.EduOutcome.DTOs
{
    public class LearningOutcomeDto : AuditedEntityDto<Guid>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal MaxMark { get; set; }
        public Guid CourseId { get; set; }
        public string Category { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public string CourseName { get; set; }
    }

    public class CreateUpdateLearningOutcomeDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal MaxMark { get; set; }
        public Guid CourseId { get; set; }
        public string Category { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class LearningOutcomeDetailDto : LearningOutcomeDto
    {
        public List<QuestionMappingDto> QuestionMappings { get; set; }
        public decimal TotalAllocatedMarks { get; set; }
        public int TotalQuestions { get; set; }
    }

    public class QuestionMappingDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public Guid AssessmentId { get; set; }
        public string AssessmentTitle { get; set; }
        public decimal AllocatedMark { get; set; }
        public decimal Percentage { get; set; }
    }
}
