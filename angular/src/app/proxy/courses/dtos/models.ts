import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CourseDto extends AuditedEntityDto<string> {
  code: string;
  name: string;
  description?: string;
  credits: number;
  programId: string;
  programName?: string;
  facultyId: string;
  facultyName?: string;
  status: string;
  isElective: boolean;
  prerequisiteCourseId?: number;
  prerequisiteCourseName?: string;
  semester?: string;
  academicYear: number;
}

export interface CourseStatisticsDto {
  totalEnrollments: number;
  activeEnrollments: number;
  averageGrade: number;
  learningOutcomesCovered: number;
}

export interface CreateCourseDto {
  code: string;
  name: string;
  description?: string;
  credits: number;
  programId: string;
  facultyId: string;
  status: string;
  isElective: boolean;
  prerequisiteCourseId?: number;
  semester?: string;
  academicYear: number;
}

export interface GetCourseListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  programId?: string;
  facultyId?: string;
  status?: string;
  semester?: number;
  academicYear?: number;
}

export interface UpdateCourseDto {
  code: string;
  name: string;
  description?: string;
  credits: number;
  programId: string;
  facultyId: string;
  status: string;
  isElective: boolean;
  prerequisiteCourseId?: number;
  semester?: string;
  academicYear: number;
}
