using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSA.EduOutcome.Entities;

namespace PSA.EduOutcome.EntityFrameworkCore.Configurations
{
    public class StudentResponseConfiguration : IEntityTypeConfiguration<StudentResponse>
    {
        public void Configure(EntityTypeBuilder<StudentResponse> builder)
        {
            builder.ToTable("StudentResponses");
            builder.HasKey(sr => sr.Id);
            builder.Property(sr => sr.Grade).IsRequired();
            builder.Property(sr => sr.Comment).HasMaxLength(1000);
        }
    }
} 