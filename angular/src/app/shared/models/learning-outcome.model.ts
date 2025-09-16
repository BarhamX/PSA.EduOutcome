export interface LearningOutcomeDto {
  id: string;
  code: string;
  description: string;
  maxMark: number;
  courseId: string;
  courseName?: string;
  category: string;
  displayOrder: number;
  isActive: boolean;
  creationTime?: Date;
  lastModificationTime?: Date;
}

export interface CreateUpdateLearningOutcomeDto {
  code: string;
  description: string;
  maxMark: number;
  courseId: string;
  category: string;
  displayOrder: number;
}

export interface LearningOutcomeDetailDto extends LearningOutcomeDto {
  questionMappings: QuestionMappingDto[];
  totalAllocatedMarks: number;
  totalQuestions: number;
}

export interface QuestionMappingDto {
  questionId: string;
  questionText: string;
  assessmentId: string;
  assessmentTitle: string;
  allocatedMark: number;
  percentage: number;
}

export interface CourseValidationResultDto {
  isValid: boolean;
  totalMarks: number;
  validationErrors: string[];
  outcomeValidations: LearningOutcomeValidationDto[];
}

export interface LearningOutcomeValidationDto {
  id: string;
  code: string;
  maxMark: number;
  hasQuestionMappings: boolean;
  issues: string[];
}

export interface GetLearningOutcomeListDto {
  skipCount?: number;
  maxResultCount?: number;
  sorting?: string;
  courseId?: string;
  category?: string;
  filter?: string;
}