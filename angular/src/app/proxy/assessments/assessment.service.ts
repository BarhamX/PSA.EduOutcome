import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { AssessmentDetailDto, AssessmentDto, CreateUpdateAssessmentDto } from '../dtos/models';
import type { AssessmentValidationResultDto } from '../models';

@Injectable({
  providedIn: 'root',
})
export class AssessmentService {
  apiName = 'Default';
  

  create = (input: CreateUpdateAssessmentDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssessmentDto>({
      method: 'POST',
      url: '/api/app/assessment',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/assessment/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssessmentDto>({
      method: 'GET',
      url: `/api/app/assessment/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getByCourse = (courseId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssessmentDto[]>({
      method: 'GET',
      url: `/api/app/assessment/by-course/${courseId}`,
    },
    { apiName: this.apiName,...config });
  

  getDetail = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssessmentDetailDto>({
      method: 'GET',
      url: `/api/app/assessment/${id}/detail`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<AssessmentDto>>({
      method: 'GET',
      url: '/api/app/assessment',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  publish = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssessmentDto>({
      method: 'POST',
      url: `/api/app/assessment/${id}/publish`,
    },
    { apiName: this.apiName,...config });
  

  unpublish = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssessmentDto>({
      method: 'POST',
      url: `/api/app/assessment/${id}/unpublish`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateAssessmentDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssessmentDto>({
      method: 'PUT',
      url: `/api/app/assessment/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  validateAssessment = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssessmentValidationResultDto>({
      method: 'POST',
      url: `/api/app/assessment/${id}/validate-assessment`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
