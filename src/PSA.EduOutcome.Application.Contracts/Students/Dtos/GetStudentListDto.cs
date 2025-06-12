using System;
using Volo.Abp.Application.Dtos;

namespace PSA.EduOutcome.Students.Dtos
{
    public class GetStudentListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public Guid? ProgramId { get; set; }
        public string? Status { get; set; }
    }
}
