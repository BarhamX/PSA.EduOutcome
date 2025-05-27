using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PSA.EduOutcome.DTOs
{
    public class CourseDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
        public string Semester { get; set; }
        public int Year { get; set; }
        public Guid ProgramId { get; set; }
        public Guid? InstructorId { get; set; }
        public bool IsActive { get; set; }
        public string ProgramName { get; set; }
        public string InstructorName { get; set; }
        public List<LearningOutcomeDto> LearningOutcomes { get; set; }
    }

    public class CreateUpdateCourseDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
        public string Semester { get; set; }
        public int Year { get; set; }
        public Guid ProgramId { get; set; }
        public Guid? InstructorId { get; set; }
    }

    public class CourseDetailDto : CourseDto
    {
        public List<AssessmentDto> Assessments { get; set; }
        public List<EnrollmentDto> Enrollments { get; set; }
        public decimal TotalLearningOutcomesMarks { get; set; }
    }
}
