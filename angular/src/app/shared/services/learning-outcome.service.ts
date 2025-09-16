import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResultDto } from '@abp/ng.core';
import { BaseApiService } from './base-api.service';
import {
  LearningOutcomeDto,
  CreateUpdateLearningOutcomeDto,
  LearningOutcomeDetailDto,
  CourseValidationResultDto,
  GetLearningOutcomeListDto
} from '../models/learning-outcome.model';

@Injectable({
  providedIn: 'root'
})
export class LearningOutcomeService extends BaseApiService {
  private readonly baseUrl = '/api/app/learning-outcomes';

  getList(input?: GetLearningOutcomeListDto): Observable<PagedResultDto<LearningOutcomeDto>> {
    return this.get<PagedResultDto<LearningOutcomeDto>>(this.baseUrl, input);
  }

  get(id: string): Observable<LearningOutcomeDto> {
    return this.get<LearningOutcomeDto>(`${this.baseUrl}/${id}`);
  }

  getDetail(id: string): Observable<LearningOutcomeDetailDto> {
    return this.get<LearningOutcomeDetailDto>(`${this.baseUrl}/${id}/detail`);
  }

  create(input: CreateUpdateLearningOutcomeDto): Observable<LearningOutcomeDto> {
    return this.post<LearningOutcomeDto>(this.baseUrl, input);
  }

  update(id: string, input: CreateUpdateLearningOutcomeDto): Observable<LearningOutcomeDto> {
    return this.put<LearningOutcomeDto>(`${this.baseUrl}/${id}`, input);
  }

  delete(id: string): Observable<void> {
    return this.delete<void>(`${this.baseUrl}/${id}`);
  }

  validateCourseMarks(courseId: string): Observable<CourseValidationResultDto> {
    return this.get<CourseValidationResultDto>(`${this.baseUrl}/validate-course/${courseId}`);
  }

  getByCourse(courseId: string): Observable<LearningOutcomeDto[]> {
    return this.get<LearningOutcomeDto[]>(`${this.baseUrl}/by-course/${courseId}`);
  }

  reorderOutcomes(courseId: string, outcomeIds: string[]): Observable<void> {
    return this.post<void>(`${this.baseUrl}/reorder`, { courseId, outcomeIds });
  }

  duplicateOutcome(id: string, targetCourseId?: string): Observable<LearningOutcomeDto> {
    const body = targetCourseId ? { targetCourseId } : {};
    return this.post<LearningOutcomeDto>(`${this.baseUrl}/${id}/duplicate`, body);
  }

  exportToExcel(courseId?: string): Observable<Blob> {
    const params = courseId ? { courseId } : undefined;
    return this.http.get(`${this.apiUrl}${this.baseUrl}/export-excel`, {
      params: this.buildParams(params),
      responseType: 'blob'
    });
  }
}