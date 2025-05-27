using System;
using Volo.Abp.Application.Dtos;

namespace PSA.EduOutcome.DTOs
{
    public class FacultyDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Guid UniversityId { get; set; }
        public bool IsActive { get; set; }
        public string UniversityName { get; set; }
    }

    public class CreateUpdateFacultyDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Guid UniversityId { get; set; }
    }
} 