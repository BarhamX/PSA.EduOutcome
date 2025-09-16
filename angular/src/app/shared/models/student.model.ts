export interface StudentDto {
  id: string;
  studentNumber: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  dateOfBirth: Date;
  gender: string;
  programId: string;
  enrollmentDate: Date;
  status: string;
  userId?: string;
  referenceNumber: string;
  creationTime?: Date;
  lastModificationTime?: Date;
}

export interface CreateStudentDto {
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  dateOfBirth: Date;
  gender: string;
  programId: string;
  referenceNumber: string;
}

export interface UpdateStudentDto extends CreateStudentDto {
  id: string;
}

export interface StudentStatisticsDto {
  totalCourses: number;
  completedCourses: number;
  averageGrade: number;
  learningOutcomesAchieved: number;
}

export interface GetStudentListDto {
  skipCount?: number;
  maxResultCount?: number;
  sorting?: string;
  programId?: string;
  status?: string;
  filter?: string;
}

export const StudentStatus = {
  ACTIVE: 'Active',
  GRADUATED: 'Graduated', 
  SUSPENDED: 'Suspended',
  WITHDRAWN: 'Withdrawn'
} as const;

export type StudentStatusType = typeof StudentStatus[keyof typeof StudentStatus];