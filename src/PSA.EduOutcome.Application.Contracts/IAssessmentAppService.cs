using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using PSA.EduOutcome.DTOs;

namespace PSA.EduOutcome
{
    public interface IAssessmentAppService : IApplicationService
    {
        Task<AssessmentDto> GetAsync(Guid id);
        Task<PagedResultDto<AssessmentDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<AssessmentDto> CreateAsync(CreateUpdateAssessmentDto input);
        Task<AssessmentDto> UpdateAsync(Guid id, CreateUpdateAssessmentDto input);
        Task DeleteAsync(Guid id);
        Task<List<AssessmentDto>> GetByCourseAsync(Guid courseId);
        Task<AssessmentDetailDto> GetDetailAsync(Guid id);
        Task<AssessmentDto> PublishAsync(Guid id);
        Task<AssessmentDto> UnpublishAsync(Guid id);
        Task<AssessmentValidationResultDto> ValidateAssessmentAsync(Guid id);
    }

    public class AssessmentValidationResultDto
    {
        public bool IsValid { get; set; }
        public List<string> ValidationErrors { get; set; } = new();
        public decimal TotalQuestionMarks { get; set; }
        public decimal AssessmentTotalMarks { get; set; }
        public int QuestionsWithoutMappings { get; set; }
        public List<QuestionValidationDto> QuestionValidations { get; set; } = new();
    }

    public class QuestionValidationDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool HasMappings { get; set; }
        public bool MappingsComplete { get; set; }
        public List<string> Issues { get; set; } = new();
    }
}
