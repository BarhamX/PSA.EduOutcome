using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using PSA.EduOutcome.DTOs;

namespace PSA.EduOutcome
{
    public interface ILearningOutcomeAppService : IApplicationService
    {
        Task<LearningOutcomeDto> GetAsync(Guid id);
        Task<PagedResultDto<LearningOutcomeDto>> GetListAsync(GetLearningOutcomeListDto input);
        Task<LearningOutcomeDto> CreateAsync(CreateUpdateLearningOutcomeDto input);
        Task<LearningOutcomeDto> UpdateAsync(Guid id, CreateUpdateLearningOutcomeDto input);
        Task DeleteAsync(Guid id);
        Task<List<LearningOutcomeDto>> GetByCourseAsync(Guid courseId);
        Task<LearningOutcomeDetailDto> GetDetailAsync(Guid id);
        Task<CourseValidationResultDto> ValidateCourseOutcomesAsync(Guid courseId);
    }

    public class CourseValidationResultDto
    {
        public bool IsValid { get; set; }
        public decimal TotalMarks { get; set; }
        public List<string> ValidationErrors { get; set; } = new();
        public List<LearningOutcomeValidationDto> OutcomeValidations { get; set; } = new();
    }

    public class LearningOutcomeValidationDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public decimal MaxMark { get; set; }
        public bool HasQuestionMappings { get; set; }
        public List<string> Issues { get; set; } = new();
    }
}

