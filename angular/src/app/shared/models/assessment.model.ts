export interface AssessmentDto {
  id: string;
  title: string;
  description: string;
  type: string;
  totalMarks: number;
  weight: number;
  courseId: string;
  courseName?: string;
  dueDate: Date;
  isPublished: boolean;
  allowLateSubmission: boolean;
  latePenaltyPercentage?: number;
  questionCount: number;
  creationTime?: Date;
  lastModificationTime?: Date;
}

export interface CreateUpdateAssessmentDto {
  title: string;
  description: string;
  type: string;
  totalMarks: number;
  weight: number;
  courseId: string;
  dueDate: Date;
  allowLateSubmission: boolean;
  latePenaltyPercentage?: number;
}

export interface AssessmentDetailDto extends AssessmentDto {
  questions: QuestionDto[];
  totalAllocatedMarks: number;
  isValid: boolean;
  validationErrors: string[];
}

export interface QuestionDto {
  id: string;
  text: string;
  maxMark: number;
  questionNumber: number;
  assessmentId: string;
  type: string;
  creationTime?: Date;
}

export interface AssessmentValidationResultDto {
  isValid: boolean;
  assessmentTotalMarks: number;
  totalQuestionMarks: number;
  questionsWithoutMappings: number;
  validationErrors: string[];
  questionValidations: QuestionValidationDto[];
}

export interface QuestionValidationDto {
  questionId: string;
  questionText: string;
  hasMappings: boolean;
  mappingsComplete: boolean;
  issues: string[];
}

export interface GetAssessmentListDto {
  skipCount?: number;
  maxResultCount?: number;
  sorting?: string;
  courseId?: string;
  type?: string;
  isPublished?: boolean;
  filter?: string;
}