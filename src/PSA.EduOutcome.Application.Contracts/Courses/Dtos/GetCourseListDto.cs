using System;
using Volo.Abp.Application.Dtos;

namespace PSA.EduOutcome.Courses.Dtos
{
    public class GetCourseListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public Guid? ProgramId { get; set; }
        public Guid? FacultyId { get; set; }
        public string? Status { get; set; }
        public int? Semester { get; set; }
        public int? AcademicYear { get; set; }
    }
}
