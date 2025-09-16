export interface CourseDto {
  id: string;
  name: string;
  code: string;
  description: string;
  credits: number;
  semester: number;
  year: number;
  programId: string;
  facultyId: string;
  status: string;
  isElective: boolean;
  academicYear: number;
  isActive: boolean;
  creationTime?: Date;
  lastModificationTime?: Date;
}

export interface CreateCourseDto {
  name: string;
  code: string;
  description: string;
  credits: number;
  semester: number;
  year: number;
  programId: string;
  facultyId: string;
  isElective?: boolean;
  academicYear?: number;
}

export interface UpdateCourseDto extends CreateCourseDto {
  id: string;
}

export interface CourseStatisticsDto {
  totalEnrollments: number;
  activeEnrollments: number;
  averageGrade: number;
  learningOutcomesCovered: number;
}

export interface GetCourseListDto {
  skipCount?: number;
  maxResultCount?: number;
  sorting?: string;
  programId?: string;
  facultyId?: string;
  status?: string;
  semester?: number;
  academicYear?: number;
  filter?: string;
}