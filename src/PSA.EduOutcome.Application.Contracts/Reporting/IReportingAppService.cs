using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PSA.EduOutcome.Reporting;

public interface IReportingAppService : IApplicationService
{
    Task<CourseReportDto> GetCourseReportAsync(Guid courseId);
}
