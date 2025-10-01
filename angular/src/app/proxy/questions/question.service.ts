import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateUpdateQuestionDto, GetQuestionListDto, QuestionDto } from '../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class QuestionService {
  apiName = 'Default';

  constructor(private restService: RestService) {}

  create = (input: CreateUpdateQuestionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionDto>({
      method: 'POST',
      url: '/api/app/questions',
      body: input,
    }, { apiName: this.apiName, ...config });

  update = (id: string, input: CreateUpdateQuestionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionDto>({
      method: 'PUT',
      url: `/api/app/questions/${id}`,
      body: input,
    }, { apiName: this.apiName, ...config });

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/questions/${id}`,
    }, { apiName: this.apiName, ...config });

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, QuestionDto>({
      method: 'GET',
      url: `/api/app/questions/${id}`,
    }, { apiName: this.apiName, ...config });

  getList = (input: GetQuestionListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<QuestionDto>>({
      method: 'GET',
      url: '/api/app/questions',
      params: { ...input },
    }, { apiName: this.apiName, ...config });
}
