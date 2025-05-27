using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSA.EduOutcome.Entities;

namespace PSA.EduOutcome.EntityFrameworkCore.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.ToTable("Enrollments");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.EnrolledAt).IsRequired();
        }
    }
} 