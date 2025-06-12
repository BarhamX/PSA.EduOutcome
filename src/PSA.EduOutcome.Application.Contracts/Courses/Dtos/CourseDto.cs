using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace PSA.EduOutcome.Courses.Dtos
{
    public class CourseDto : AuditedEntityDto<Guid>
    {
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public int Credits { get; set; }

        [Required]
        public Guid ProgramId { get; set; }

        public string ProgramName { get; set; }

        [Required]
        public Guid FacultyId { get; set; }

        public string FacultyName { get; set; }

        [Required]
        public string Status { get; set; } // Active, Inactive, Archived

        public bool IsElective { get; set; }

        public int? PrerequisiteCourseId { get; set; }

        public string PrerequisiteCourseName { get; set; }

        public string Semester { get; set; }

        public int AcademicYear { get; set; }
    }
} 