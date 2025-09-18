import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { StudentDto } from '../students/dtos/models';

@Injectable({
  providedIn: 'root',
})
export class StudentsService {
  apiName = 'Default';
  

  getById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StudentDto>({
      method: 'GET',
      url: `/api/students/${id}`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
