import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface StudentDto extends AuditedEntityDto<string> {
  studentNumber: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  dateOfBirth: string;
  gender: string;
  programId: string;
  programName?: string;
  enrollmentDate: string;
  status: string;
  userId?: string;
  referenceNumber: string;
  fullName?: string;
}

export interface CreateStudentDto {
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  dateOfBirth: string;
  gender: string;
  programId: string;
  referenceNumber: string;
}

export interface GetStudentListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  programId?: string;
  status?: string;
}

export interface StudentStatisticsDto {
  totalCourses: number;
  completedCourses: number;
  averageGrade: number;
  learningOutcomesAchieved: number;
}

export interface UpdateStudentDto {
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  dateOfBirth: string;
  gender: string;
  programId: string;
  referenceNumber: string;
}
