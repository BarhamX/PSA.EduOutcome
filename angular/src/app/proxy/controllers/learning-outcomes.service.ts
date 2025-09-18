import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateUpdateLearningOutcomeDto, LearningOutcomeDetailDto, LearningOutcomeDto } from '../dtos/models';
import type { CourseValidationResultDto } from '../models';

@Injectable({
  providedIn: 'root',
})
export class LearningOutcomesService {
  apiName = 'Default';
  

  create = (input: CreateUpdateLearningOutcomeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LearningOutcomeDto>({
      method: 'POST',
      url: '/api/app/learning-outcomes',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/learning-outcomes/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LearningOutcomeDto>({
      method: 'GET',
      url: `/api/app/learning-outcomes/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getByCourse = (courseId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LearningOutcomeDto[]>({
      method: 'GET',
      url: `/api/app/learning-outcomes/by-course/${courseId}`,
    },
    { apiName: this.apiName,...config });
  

  getDetail = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LearningOutcomeDetailDto>({
      method: 'GET',
      url: `/api/app/learning-outcomes/${id}/detail`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<LearningOutcomeDto>>({
      method: 'GET',
      url: '/api/app/learning-outcomes',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateLearningOutcomeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LearningOutcomeDto>({
      method: 'PUT',
      url: `/api/app/learning-outcomes/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  validateCourseOutcomes = (courseId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseValidationResultDto>({
      method: 'POST',
      url: `/api/app/learning-outcomes/validate-course/${courseId}`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
