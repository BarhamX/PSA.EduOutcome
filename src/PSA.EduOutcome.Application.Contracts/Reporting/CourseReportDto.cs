using System.Collections.Generic;

namespace PSA.EduOutcome.Reporting;

public class CourseReportDto
{
    public decimal AverageGrade { get; set; }
    public int EnrolledStudents { get; set; }
    public Dictionary<string, int> GradeDistribution { get; set; } = new();
}
