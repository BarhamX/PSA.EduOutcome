import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CourseDto } from '../courses/dtos/models';
import type { CourseReportDto } from '../reporting/models';

@Injectable({
  providedIn: 'root',
})
export class CoursesService {
  apiName = 'Default';
  

  getElectivesByProgramIdAndSemester = (programId: string, semester: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseDto[]>({
      method: 'GET',
      url: `/api/courses/${programId}/semester/${semester}/electives`,
    },
    { apiName: this.apiName,...config });
  

  getReportById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseReportDto>({
      method: 'GET',
      url: `/api/courses/${id}/report`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
