import type { CourseDto, CourseStatisticsDto, CreateCourseDto, GetCourseListDto, UpdateCourseDto } from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  apiName = 'Default';
  

  activate = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseDto>({
      method: 'POST',
      url: `/api/app/course/${id}/activate`,
    },
    { apiName: this.apiName,...config });
  

  archive = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseDto>({
      method: 'POST',
      url: `/api/app/course/${id}/archive`,
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreateCourseDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseDto>({
      method: 'POST',
      url: '/api/app/course',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  deactivate = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseDto>({
      method: 'POST',
      url: `/api/app/course/${id}/deactivate`,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/course/${id}`,
    },
    { apiName: this.apiName,...config });
  

  exportToExcel = (input: GetCourseListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, number[]>({
      method: 'POST',
      url: '/api/app/course/export-to-excel',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseDto>({
      method: 'GET',
      url: `/api/app/course/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getByCode = (code: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseDto>({
      method: 'GET',
      url: '/api/app/course/by-code',
      params: { code },
    },
    { apiName: this.apiName,...config });
  

  getByFaculty = (facultyId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseDto[]>({
      method: 'GET',
      url: `/api/app/course/by-faculty/${facultyId}`,
    },
    { apiName: this.apiName,...config });
  

  getByProgram = (programId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseDto[]>({
      method: 'GET',
      url: `/api/app/course/by-program/${programId}`,
    },
    { apiName: this.apiName,...config });
  

  getElectiveCourses = (programId: string, semester: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseDto[]>({
      method: 'GET',
      url: `/api/app/course/elective-courses/${programId}`,
      params: { semester },
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetCourseListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CourseDto>>({
      method: 'GET',
      url: '/api/app/course',
      params: { filter: input.filter, programId: input.programId, facultyId: input.facultyId, status: input.status, semester: input.semester, academicYear: input.academicYear, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPrerequisites = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseDto[]>({
      method: 'GET',
      url: `/api/app/course/${id}/prerequisites`,
    },
    { apiName: this.apiName,...config });
  

  getStatistics = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseStatisticsDto>({
      method: 'GET',
      url: `/api/app/course/${id}/statistics`,
    },
    { apiName: this.apiName,...config });
  

  importFromExcel = (fileContent: number[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/course/import-from-excel',
      body: fileContent,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateCourseDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseDto>({
      method: 'PUT',
      url: `/api/app/course/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
