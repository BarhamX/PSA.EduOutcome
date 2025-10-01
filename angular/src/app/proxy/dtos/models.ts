import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface AssessmentDetailDto extends AssessmentDto {
  questions: QuestionDto[];
  totalAllocatedMarks: number;
  isValid: boolean;
  validationErrors: string[];
}

export interface AssessmentDto extends AuditedEntityDto<string> {
  title?: string;
  description?: string;
  type?: string;
  totalMarks: number;
  weight: number;
  courseId?: string;
  dueDate?: string;
  isPublished: boolean;
  allowLateSubmission: boolean;
  latePenaltyPercentage?: number;
  courseName?: string;
  questionCount: number;
}

export interface CreateUpdateAssessmentDto {
  title?: string;
  description?: string;
  type?: string;
  totalMarks: number;
  weight: number;
  courseId?: string;
  dueDate?: string;
  allowLateSubmission: boolean;
  latePenaltyPercentage?: number;
}

export interface CreateUpdateLearningOutcomeDto {
  code?: string;
  description?: string;
  maxMark: number;
  courseId?: string;
  category?: string;
  displayOrder: number;
}

export interface GetLearningOutcomeListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  courseId?: string;
  category?: string;
}

export interface LearningOutcomeDetailDto extends LearningOutcomeDto {
  questionMappings: QuestionMappingDto[];
  totalAllocatedMarks: number;
  totalQuestions: number;
}

export interface LearningOutcomeDto extends AuditedEntityDto<string> {
  code?: string;
  description?: string;
  maxMark: number;
  courseId?: string;
  category?: string;
  displayOrder: number;
  isActive: boolean;
  courseName?: string;
}

export interface QuestionDto extends AuditedEntityDto<string> {
  text?: string;
  description?: string;
  questionNumber: number;
  maxMark: number;
  assessmentId?: string;
  questionType?: string;
  assessmentTitle?: string;
  learningOutcomeMappings: QuestionLearningOutcomeDto[];
}

export interface QuestionLearningOutcomeDto {
  id?: string;
  questionId?: string;
  learningOutcomeId?: string;
  allocatedMark: number;
  percentage: number;
  learningOutcomeCode?: string;
  learningOutcomeDescription?: string;
}

export interface QuestionMappingDto {
  questionId?: string;
  questionText?: string;
  assessmentId?: string;
  assessmentTitle?: string;
  allocatedMark: number;
  percentage: number;
}

