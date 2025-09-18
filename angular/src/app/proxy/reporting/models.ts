
export interface CourseReportDto {
  averageGrade: number;
  enrolledStudents: number;
  gradeDistribution: Record<string, number>;
}
