using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSA.EduOutcome.Controllers;
using PSA.EduOutcome.Courses;
using PSA.EduOutcome.Reporting;

namespace PSA.EduOutcome.Controllers;

[Route("api/courses")]
public class CoursesController : EduOutcomeController
{
    private readonly ICourseAppService _courseAppService;
    private readonly IReportingAppService _reportingAppService;

    public CoursesController(ICourseAppService courseAppService, IReportingAppService reportingAppService)
    {
        _courseAppService = courseAppService;
        _reportingAppService = reportingAppService;
    }

    [HttpGet("{programId}/semester/{semester}/electives")]
    public Task<List<CourseDto>> GetElectives(Guid programId, string semester)
    {
        return _courseAppService.GetElectiveCoursesAsync(programId, semester);
    }

    [HttpGet("{id}/report")]
    public Task<CourseReportDto> GetReport(Guid id)
    {
        return _reportingAppService.GetCourseReportAsync(id);
    }
}
