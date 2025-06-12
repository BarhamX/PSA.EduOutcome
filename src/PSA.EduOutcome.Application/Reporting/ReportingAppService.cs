using System;
using System.Linq;
using System.Threading.Tasks;
using PSA.EduOutcome.Courses;
using PSA.EduOutcome.Domain.Repositories;
using PSA.EduOutcome.Reporting;
using Volo.Abp.Application.Services;

namespace PSA.EduOutcome.Reporting;

public class ReportingAppService : ApplicationService, IReportingAppService
{
    private readonly ICourseRepository _courseRepository;

    public ReportingAppService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<CourseReportDto> GetCourseReportAsync(Guid courseId)
    {
        var course = await _courseRepository.GetWithDetailsAsync(courseId);

        var grades = course.Enrollments
            .Where(e => e.FinalGrade.HasValue)
            .Select(e => e.FinalGrade.Value)
            .ToList();

        var distribution = new System.Collections.Generic.Dictionary<string, int>
        {
            ["A"] = 0,
            ["B"] = 0,
            ["C"] = 0,
            ["D"] = 0,
            ["F"] = 0
        };

        foreach (var grade in grades)
        {
            if (grade >= 90) distribution["A"]++;
            else if (grade >= 80) distribution["B"]++;
            else if (grade >= 70) distribution["C"]++;
            else if (grade >= 60) distribution["D"]++;
            else distribution["F"]++;
        }

        return new CourseReportDto
        {
            AverageGrade = grades.Any() ? grades.Average() : 0,
            EnrolledStudents = course.Enrollments.Count,
            GradeDistribution = distribution
        };
    }
}
