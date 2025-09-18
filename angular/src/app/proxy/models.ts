
export interface AssessmentValidationResultDto {
  isValid: boolean;
  validationErrors: string[];
  totalQuestionMarks: number;
  assessmentTotalMarks: number;
  questionsWithoutMappings: number;
  questionValidations: QuestionValidationDto[];
}

export interface CourseValidationResultDto {
  isValid: boolean;
  totalMarks: number;
  validationErrors: string[];
  outcomeValidations: LearningOutcomeValidationDto[];
}

export interface LearningOutcomeValidationDto {
  id?: string;
  code?: string;
  maxMark: number;
  hasQuestionMappings: boolean;
  issues: string[];
}

export interface QuestionValidationDto {
  questionId?: string;
  questionText?: string;
  hasMappings: boolean;
  mappingsComplete: boolean;
  issues: string[];
}
