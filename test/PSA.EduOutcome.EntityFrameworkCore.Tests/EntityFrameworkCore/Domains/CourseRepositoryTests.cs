using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace PSA.EduOutcome.EntityFrameworkCore.Domains;

[Collection(EduOutcomeTestConsts.CollectionDefinitionName)]
public class CourseRepositoryTests : EduOutcomeEntityFrameworkCoreTestBase
{
    private readonly ICourseRepository _courseRepository;
    private readonly IRepository<Course, Guid> _courseRepo;
    private readonly IRepository<Program, Guid> _programRepo;
    private readonly IRepository<Faculty, Guid> _facultyRepo;
    private readonly IRepository<University, Guid> _universityRepo;

    public CourseRepositoryTests()
    {
        _courseRepository = GetRequiredService<ICourseRepository>();
        _courseRepo = GetRequiredService<IRepository<Course, Guid>>();
        _programRepo = GetRequiredService<IRepository<Program, Guid>>();
        _facultyRepo = GetRequiredService<IRepository<Faculty, Guid>>();
        _universityRepo = GetRequiredService<IRepository<University, Guid>>();
    }

    [Fact]
    public async Task Should_Filter_By_Semester_String()
    {
        var programId = Guid.NewGuid();
        await WithUnitOfWorkAsync(async () =>
        {
            var uni = await _universityRepo.InsertAsync(new University(Guid.NewGuid(), "Test Uni", "TU"));
            var fac = await _facultyRepo.InsertAsync(new Faculty(Guid.NewGuid(), "Fac", "FAC", uni.Id));
            await _programRepo.InsertAsync(new Program(programId, "Prog", "PRG", "BSc", 8, 120, fac.Id));
            await _courseRepo.InsertAsync(new Course(Guid.NewGuid(), "Course1", "C1", 3, "Fall", 2024, programId));
            await _courseRepo.InsertAsync(new Course(Guid.NewGuid(), "Course2", "C2", 3, "Spring", 2024, programId));
        });

        var result = await WithUnitOfWorkAsync(() => _courseRepository.GetElectiveCoursesAsync(programId, "Fall"));

        result.ShouldAllBe(c => c.Semester == "Fall");
    }
}
