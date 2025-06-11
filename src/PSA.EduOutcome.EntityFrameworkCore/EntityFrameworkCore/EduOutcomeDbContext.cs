using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using PSA.EduOutcome.Entities;

namespace PSA.EduOutcome.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class EduOutcomeDbContext :
    AbpDbContext<EduOutcomeDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    
    // Domain entities
    public DbSet<University> Universities { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Entities.Program> Programs { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<LearningOutcome> LearningOutcomes { get; set; }
    public DbSet<Assessment> Assessments { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionLearningOutcome> QuestionLearningOutcomes { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<StudentResponse> StudentResponses { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public EduOutcomeDbContext(DbContextOptions<EduOutcomeDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();
        
        /* Configure your own tables/entities inside here */

        // Configure University
        builder.Entity<University>(b =>
        {
            b.ToTable(EduOutcomeConsts.DbTablePrefix + "Universities", EduOutcomeConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.Code).IsRequired().HasMaxLength(50);
            b.Property(x => x.ContactEmail).HasMaxLength(256);
            b.Property(x => x.ContactPhone).HasMaxLength(50);
            
            b.HasIndex(x => x.Code).IsUnique();
        });

        // Configure Faculty
        builder.Entity<Faculty>(b =>
        {
            b.ToTable(EduOutcomeConsts.DbTablePrefix + "Faculties", EduOutcomeConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.Code).IsRequired().HasMaxLength(50);
            
            b.HasOne(x => x.University)
                .WithMany(x => x.Faculties)
                .HasForeignKey(x => x.UniversityId)
                .OnDelete(DeleteBehavior.Cascade);
                
            b.HasIndex(x => new { x.Code, x.UniversityId }).IsUnique();
        });

        // Configure Program
        builder.Entity<Entities.Program>(b =>
        {
            b.ToTable(EduOutcomeConsts.DbTablePrefix + "Programs", EduOutcomeConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.Code).IsRequired().HasMaxLength(50);
            b.Property(x => x.Degree).IsRequired().HasMaxLength(100);
            
            b.HasOne(x => x.Faculty)
                .WithMany(x => x.Programs)
                .HasForeignKey(x => x.FacultyId)
                .OnDelete(DeleteBehavior.Cascade);
                
            b.HasIndex(x => new { x.Code, x.FacultyId }).IsUnique();
        });

        // Configure Course
        builder.Entity<Course>(b =>
        {
            b.ToTable(EduOutcomeConsts.DbTablePrefix + "Courses", EduOutcomeConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.Code).IsRequired().HasMaxLength(50);
            b.Property(x => x.Semester).IsRequired().HasMaxLength(20);
            
            b.HasOne(x => x.Program)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);
                
            b.HasIndex(x => new { x.Code, x.ProgramId, x.Semester, x.Year }).IsUnique();
        });

        // Configure Student
        builder.Entity<Student>(b =>
        {
            b.ToTable(EduOutcomeConsts.DbTablePrefix + "Students", EduOutcomeConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.StudentNumber).IsRequired().HasMaxLength(50);
            b.Property(x => x.ReferenceNumber).IsRequired().HasMaxLength(50);
            b.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            b.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            b.Property(x => x.Email).IsRequired().HasMaxLength(256);
            b.Property(x => x.Phone).HasMaxLength(50);
            b.Property(x => x.Gender).IsRequired().HasMaxLength(10);
            b.Property(x => x.Status).IsRequired().HasMaxLength(20);
            
            b.HasOne(x => x.Program)
                .WithMany()
                .HasForeignKey(x => x.ProgramId)
                .OnDelete(DeleteBehavior.Restrict);
            
            b.HasIndex(x => x.StudentNumber).IsUnique();
            b.HasIndex(x => x.ReferenceNumber).IsUnique();
            b.HasIndex(x => x.Email).IsUnique();
        });

        // Configure LearningOutcome
        builder.Entity<LearningOutcome>(b =>
        {
            b.ToTable(EduOutcomeConsts.DbTablePrefix + "LearningOutcomes", EduOutcomeConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Code).IsRequired().HasMaxLength(20);
            b.Property(x => x.Description).IsRequired().HasMaxLength(1000);
            b.Property(x => x.MaxMark).HasPrecision(5, 2);
            b.Property(x => x.Category).IsRequired().HasMaxLength(50);
            
            b.HasOne(x => x.Course)
                .WithMany(x => x.LearningOutcomes)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
                
            b.HasIndex(x => new { x.Code, x.CourseId }).IsUnique();
        });

        // Configure Assessment
        builder.Entity<Assessment>(b =>
        {
            b.ToTable(EduOutcomeConsts.DbTablePrefix + "Assessments", EduOutcomeConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Title).IsRequired().HasMaxLength(200);
            b.Property(x => x.Type).IsRequired().HasMaxLength(50);
            b.Property(x => x.TotalMarks).HasPrecision(5, 2);
            b.Property(x => x.Weight).HasPrecision(5, 2);
            
            b.HasOne(x => x.Course)
                .WithMany(x => x.Assessments)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Question
        builder.Entity<Question>(b =>
        {
            b.ToTable(EduOutcomeConsts.DbTablePrefix + "Questions", EduOutcomeConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Text).IsRequired().HasMaxLength(2000);
            b.Property(x => x.MaxMark).HasPrecision(5, 2);
            b.Property(x => x.QuestionType).IsRequired().HasMaxLength(50);
            
            b.HasOne(x => x.Assessment)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.AssessmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure QuestionLearningOutcome
        builder.Entity<QuestionLearningOutcome>(b =>
        {
            b.ToTable(EduOutcomeConsts.DbTablePrefix + "QuestionLearningOutcomes", EduOutcomeConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.AllocatedMark).HasPrecision(5, 2);
            b.Property(x => x.Percentage).HasPrecision(5, 2);
            
            b.HasOne(x => x.Question)
                .WithMany(x => x.QuestionLearningOutcomes)
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
                
            b.HasOne(x => x.LearningOutcome)
                .WithMany(x => x.QuestionMappings)
                .HasForeignKey(x => x.LearningOutcomeId)
                .OnDelete(DeleteBehavior.Restrict);
                
            b.HasIndex(x => new { x.QuestionId, x.LearningOutcomeId }).IsUnique();
        });

        // Configure Enrollment
        builder.Entity<Enrollment>(b =>
        {
            b.ToTable(EduOutcomeConsts.DbTablePrefix + "Enrollments", EduOutcomeConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Status).IsRequired().HasMaxLength(20);
            b.Property(x => x.FinalGrade).HasPrecision(5, 2);
            b.Property(x => x.LetterGrade).HasMaxLength(5);
            b.Property(x => x.GradePoints).HasPrecision(3, 2);
            
            b.HasOne(x => x.Student)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
                
            b.HasOne(x => x.Course)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
                
            b.HasIndex(x => new { x.StudentId, x.CourseId }).IsUnique();
        });

        // Configure StudentResponse
        builder.Entity<StudentResponse>(b =>
        {
            b.ToTable(EduOutcomeConsts.DbTablePrefix + "StudentResponses", EduOutcomeConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.ResponseText).HasMaxLength(4000);
            b.Property(x => x.AchievedMark).HasPrecision(5, 2);
            b.Property(x => x.GraderComments).HasMaxLength(1000);
            
            b.HasOne(x => x.Student)
                .WithMany(x => x.Responses)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
                
            b.HasOne(x => x.Question)
                .WithMany(x => x.StudentResponses)
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
                
            b.HasOne(x => x.Enrollment)
                .WithMany()
                .HasForeignKey(x => x.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);
                
            b.HasIndex(x => new { x.StudentId, x.QuestionId }).IsUnique();
        });
    }
}
