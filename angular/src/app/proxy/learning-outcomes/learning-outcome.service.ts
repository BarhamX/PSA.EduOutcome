import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateUpdateLearningOutcomeDto, GetLearningOutcomeListDto, LearningOutcomeDetailDto, LearningOutcomeDto } from '../dtos/models';
import type { CourseValidationResultDto } from '../models';

@Injectable({
  providedIn: 'root',
})
export class LearningOutcomeService {
  apiName = 'Default';
  

  create = (input: CreateUpdateLearningOutcomeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LearningOutcomeDto>({
      method: 'POST',
      url: '/api/app/learning-outcome',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/learning-outcome/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LearningOutcomeDto>({
      method: 'GET',
      url: `/api/app/learning-outcome/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getByCourse = (courseId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LearningOutcomeDto[]>({
      method: 'GET',
      url: `/api/app/learning-outcome/by-course/${courseId}`,
    },
    { apiName: this.apiName,...config });
  

  getDetail = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LearningOutcomeDetailDto>({
      method: 'GET',
      url: `/api/app/learning-outcome/${id}/detail`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetLearningOutcomeListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<LearningOutcomeDto>>({
      method: 'GET',
      url: '/api/app/learning-outcome',
      params: { filter: input.filter, courseId: input.courseId, category: input.category, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateLearningOutcomeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LearningOutcomeDto>({
      method: 'PUT',
      url: `/api/app/learning-outcome/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  validateCourseOutcomes = (courseId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseValidationResultDto>({
      method: 'POST',
      url: `/api/app/learning-outcome/validate-course-outcomes/${courseId}`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
