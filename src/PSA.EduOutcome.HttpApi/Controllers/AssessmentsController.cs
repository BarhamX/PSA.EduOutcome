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
    [Route("api/app/assessments")]
    public class AssessmentsController : AbpControllerBase
    {
        private readonly IAssessmentAppService _assessmentAppService;

        public AssessmentsController(IAssessmentAppService assessmentAppService)
        {
            _assessmentAppService = assessmentAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<AssessmentDto> GetAsync(Guid id)
        {
            return _assessmentAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<AssessmentDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _assessmentAppService.GetListAsync(input);
        }

        [HttpPost]
        public virtual Task<AssessmentDto> CreateAsync(CreateUpdateAssessmentDto input)
        {
            return _assessmentAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<AssessmentDto> UpdateAsync(Guid id, CreateUpdateAssessmentDto input)
        {
            return _assessmentAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _assessmentAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("by-course/{courseId}")]
        public virtual Task<List<AssessmentDto>> GetByCourseAsync(Guid courseId)
        {
            return _assessmentAppService.GetByCourseAsync(courseId);
        }

        [HttpGet]
        [Route("{id}/detail")]
        public virtual Task<AssessmentDetailDto> GetDetailAsync(Guid id)
        {
            return _assessmentAppService.GetDetailAsync(id);
        }

        [HttpPost]
        [Route("{id}/publish")]
        public virtual Task<AssessmentDto> PublishAsync(Guid id)
        {
            return _assessmentAppService.PublishAsync(id);
        }

        [HttpPost]
        [Route("{id}/unpublish")]
        public virtual Task<AssessmentDto> UnpublishAsync(Guid id)
        {
            return _assessmentAppService.UnpublishAsync(id);
        }

        [HttpPost]
        [Route("{id}/validate")]
        public virtual Task<AssessmentValidationResultDto> ValidateAssessmentAsync(Guid id)
        {
            return _assessmentAppService.ValidateAssessmentAsync(id);
        }
    }
}
