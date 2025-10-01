using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using PSA.EduOutcome.DTOs;

namespace PSA.EduOutcome.Controllers
{
    [RemoteService(Name = "Default")]
    [Area("app")]
    [Route("api/app/questions")]
    public class QuestionsController : AbpControllerBase
    {
        private readonly IQuestionAppService _questionAppService;

        public QuestionsController(IQuestionAppService questionAppService)
        {
            _questionAppService = questionAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<QuestionDto> GetAsync(Guid id)
        {
            return _questionAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<QuestionDto>> GetListAsync([FromQuery] GetQuestionListDto input)
        {
            return _questionAppService.GetListAsync(input);
        }

        [HttpPost]
        public virtual Task<QuestionDto> CreateAsync(CreateUpdateQuestionDto input)
        {
            return _questionAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<QuestionDto> UpdateAsync(Guid id, CreateUpdateQuestionDto input)
        {
            return _questionAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _questionAppService.DeleteAsync(id);
        }
    }
}
