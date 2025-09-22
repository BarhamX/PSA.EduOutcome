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

export interface StudentCourseAchievementDto {
  courseId?: string;
  courseName?: string;
  courseCode?: string;
  achievementPercentage: number;
  learningOutcomeAchievements: StudentLearningOutcomeAchievementDto[];
}

export interface StudentLearningOutcomeAchievementDto {
  learningOutcomeId?: string;
  code?: string;
  description?: string;
  achievementPercentage: number;
}

export interface StudentStatisticsDto {
  totalCourses: number;
  completedCourses: number;
  activeCourses: number;
  averageGrade: number;
  learningOutcomesAchieved: number;
  averageOutcomeAchievement: number;
  courseAchievements: StudentCourseAchievementDto[];
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
