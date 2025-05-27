using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSA.EduOutcome.Entities;

namespace PSA.EduOutcome.EntityFrameworkCore.Configurations
{
    public class QuestionLearningOutcomeConfiguration : IEntityTypeConfiguration<QuestionLearningOutcome>
    {
        public void Configure(EntityTypeBuilder<QuestionLearningOutcome> builder)
        {
            builder.ToTable("QuestionLearningOutcomes");
            builder.HasKey(qlo => qlo.Id);
            builder.Property(qlo => qlo.AllocatedMark).IsRequired();
        }
    }
} 