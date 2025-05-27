using System;
using Volo.Abp.Application.Dtos;

namespace PSA.EduOutcome.DTOs
{
    public class ProgramDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Degree { get; set; }
        public int DurationInSemesters { get; set; }
        public int TotalCredits { get; set; }
        public Guid FacultyId { get; set; }
        public bool IsActive { get; set; }
        public string FacultyName { get; set; }
    }

    public class CreateUpdateProgramDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Degree { get; set; }
        public int DurationInSemesters { get; set; }
        public int TotalCredits { get; set; }
        public Guid FacultyId { get; set; }
    }
} 