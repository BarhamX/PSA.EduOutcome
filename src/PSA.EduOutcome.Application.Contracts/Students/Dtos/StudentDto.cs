using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace PSA.EduOutcome.Students.Dtos
{
    public class StudentDto : AuditedEntityDto<Guid>
    {
        [Required]
        [StringLength(50)]
        public string StudentNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        public Guid ProgramId { get; set; }

        public string ProgramName { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public Guid? UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
} 