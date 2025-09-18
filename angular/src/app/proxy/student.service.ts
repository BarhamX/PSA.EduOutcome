import type { CreateStudentDto, GetStudentListDto, StudentDto, StudentStatisticsDto, UpdateStudentDto } from './students/dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  apiName = 'Default';
  

  create = (input: CreateStudentDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StudentDto>({
      method: 'POST',
      url: '/api/app/student',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/student/${id}`,
    },
    { apiName: this.apiName,...config });
  

  exportToExcel = (input: GetStudentListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, number[]>({
      method: 'POST',
      url: '/api/app/student/export-to-excel',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StudentDto>({
      method: 'GET',
      url: `/api/app/student/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getByProgram = (programId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StudentDto[]>({
      method: 'GET',
      url: `/api/app/student/by-program/${programId}`,
    },
    { apiName: this.apiName,...config });
  

  getByStudentNumber = (studentNumber: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StudentDto>({
      method: 'GET',
      url: '/api/app/student/by-student-number',
      params: { studentNumber },
    },
    { apiName: this.apiName,...config });
  

  getByUserId = (userId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StudentDto>({
      method: 'GET',
      url: `/api/app/student/by-user-id/${userId}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetStudentListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<StudentDto>>({
      method: 'GET',
      url: '/api/app/student',
      params: { filter: input.filter, programId: input.programId, status: input.status, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getStatistics = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StudentStatisticsDto>({
      method: 'GET',
      url: `/api/app/student/${id}/statistics`,
    },
    { apiName: this.apiName,...config });
  

  graduate = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StudentDto>({
      method: 'POST',
      url: `/api/app/student/${id}/graduate`,
    },
    { apiName: this.apiName,...config });
  

  importFromExcel = (fileContent: number[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/student/import-from-excel',
      body: fileContent,
    },
    { apiName: this.apiName,...config });
  

  reactivate = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StudentDto>({
      method: 'POST',
      url: `/api/app/student/${id}/reactivate`,
    },
    { apiName: this.apiName,...config });
  

  suspend = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StudentDto>({
      method: 'POST',
      url: `/api/app/student/${id}/suspend`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateStudentDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StudentDto>({
      method: 'PUT',
      url: `/api/app/student/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  withdraw = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StudentDto>({
      method: 'POST',
      url: `/api/app/student/${id}/withdraw`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
