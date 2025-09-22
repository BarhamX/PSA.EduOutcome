using System;
using System.Collections.Generic;
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
    [Route("api/app/learning-outcomes")]
    public class LearningOutcomesController : AbpControllerBase
    {
        private readonly ILearningOutcomeAppService _learningOutcomeAppService;

        public LearningOutcomesController(ILearningOutcomeAppService learningOutcomeAppService)
        {
            _learningOutcomeAppService = learningOutcomeAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<LearningOutcomeDto> GetAsync(Guid id)
        {
            return _learningOutcomeAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<LearningOutcomeDto>> GetListAsync(GetLearningOutcomeListDto input)
        {
            return _learningOutcomeAppService.GetListAsync(input);
        }

        [HttpPost]
        public virtual Task<LearningOutcomeDto> CreateAsync(CreateUpdateLearningOutcomeDto input)
        {
            return _learningOutcomeAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<LearningOutcomeDto> UpdateAsync(Guid id, CreateUpdateLearningOutcomeDto input)
        {
            return _learningOutcomeAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _learningOutcomeAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("by-course/{courseId}")]
        public virtual Task<List<LearningOutcomeDto>> GetByCourseAsync(Guid courseId)
        {
            return _learningOutcomeAppService.GetByCourseAsync(courseId);
        }

        [HttpGet]
        [Route("{id}/detail")]
        public virtual Task<LearningOutcomeDetailDto> GetDetailAsync(Guid id)
        {
            return _learningOutcomeAppService.GetDetailAsync(id);
        }

        [HttpPost]
        [Route("validate-course/{courseId}")]
        public virtual Task<CourseValidationResultDto> ValidateCourseOutcomesAsync(Guid courseId)
        {
            return _learningOutcomeAppService.ValidateCourseOutcomesAsync(courseId);
        }
    }
}
