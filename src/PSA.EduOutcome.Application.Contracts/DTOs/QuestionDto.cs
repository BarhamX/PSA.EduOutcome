using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace PSA.EduOutcome.DTOs
{
    public class QuestionDto : AuditedEntityDto<Guid>
    {
        public string Text { get; set; }
        public string Description { get; set; }
        public int QuestionNumber { get; set; }
        public decimal MaxMark { get; set; }
        public Guid AssessmentId { get; set; }
        public string QuestionType { get; set; }
        public string AssessmentTitle { get; set; }
        public List<QuestionLearningOutcomeDto> LearningOutcomeMappings { get; set; }
    }

    public class CreateUpdateQuestionDto
    {
        public string Text { get; set; }
        public string Description { get; set; }
        public int QuestionNumber { get; set; }
        public decimal MaxMark { get; set; }
        public Guid AssessmentId { get; set; }
        public string QuestionType { get; set; }
    }

    public class QuestionLearningOutcomeDto
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public Guid LearningOutcomeId { get; set; }
        public decimal AllocatedMark { get; set; }
        public decimal Percentage { get; set; }
        public string LearningOutcomeCode { get; set; }
        public string LearningOutcomeDescription { get; set; }
    }

    public class CreateQuestionLearningOutcomeDto
    {
        public Guid LearningOutcomeId { get; set; }
        public decimal AllocatedMark { get; set; }
        public decimal Percentage { get; set; }
    }
} 