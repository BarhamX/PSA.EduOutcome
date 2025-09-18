import type { CourseReportDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ReportingService {
  apiName = 'Default';
  

  getCourseReport = (courseId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourseReportDto>({
      method: 'GET',
      url: `/api/app/reporting/course-report/${courseId}`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
