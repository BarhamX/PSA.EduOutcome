using System;
using System.ComponentModel.DataAnnotations;

namespace PSA.EduOutcome.Courses.Dtos
{
    public class CreateCourseDto
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

        [Required]
        public Guid FacultyId { get; set; }

        [Required]
        public string Status { get; set; } = "Active";

        public bool IsElective { get; set; } = false;

        public int? PrerequisiteCourseId { get; set; }

        public string Semester { get; set; }

        public int AcademicYear { get; set; }
    }

    public class UpdateCourseDto
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

        [Required]
        public Guid FacultyId { get; set; }

        [Required]
        public string Status { get; set; }

        public bool IsElective { get; set; }

        public int? PrerequisiteCourseId { get; set; }

        public string Semester { get; set; }

        public int AcademicYear { get; set; }
    }
}