using System;
using System.Threading.Tasks;
using PSA.EduOutcome.DTOs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PSA.EduOutcome
{
    public interface IQuestionAppService : IApplicationService
    {
        Task<QuestionDto> GetAsync(Guid id);
        Task<PagedResultDto<QuestionDto>> GetListAsync(GetQuestionListDto input);
        Task<QuestionDto> CreateAsync(CreateUpdateQuestionDto input);
        Task<QuestionDto> UpdateAsync(Guid id, CreateUpdateQuestionDto input);
        Task DeleteAsync(Guid id);
    }
}
