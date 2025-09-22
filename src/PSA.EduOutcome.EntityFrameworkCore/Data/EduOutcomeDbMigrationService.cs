using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PSA.EduOutcome.Entities;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace PSA.EduOutcome.Data
{
    public class EduOutcomeDataSeeder : IDataSeeder, ITransientDependency
    {
        private const string DefaultUniversityCode = "DEFAULT-UNI";
        private const string DefaultFacultyCode = "FCS";

        private readonly ILogger<EduOutcomeDataSeeder> _logger;
        private readonly IRepository<Student, Guid> _studentRepository;
        private readonly IRepository<Program, Guid> _programRepository;
        private readonly IRepository<Course, Guid> _courseRepository;
        private readonly IRepository<Faculty, Guid> _facultyRepository;
        private readonly IRepository<University, Guid> _universityRepository;

        public EduOutcomeDataSeeder(
            ILogger<EduOutcomeDataSeeder> logger,
            IRepository<Student, Guid> studentRepository,
            IRepository<Program, Guid> programRepository,
            IRepository<Course, Guid> courseRepository,
            IRepository<Faculty, Guid> facultyRepository,
            IRepository<University, Guid> universityRepository)
        {
            _logger = logger;
            _studentRepository = studentRepository;
            _programRepository = programRepository;
            _courseRepository = courseRepository;
            _facultyRepository = facultyRepository;
            _universityRepository = universityRepository;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            if (string.Equals(context["DataSeed:IsSeeded"] as string, "true", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var university = await EnsureDefaultUniversityAsync();
            var faculty = await EnsureDefaultFacultyAsync(university.Id);

            await SeedProgramsAsync(faculty);
            await SeedCoursesAsync();
            await SeedStudentsAsync();

            context["DataSeed:IsSeeded"] = "true";
        }

        private async Task<University> EnsureDefaultUniversityAsync()
        {
            var existing = await _universityRepository.FirstOrDefaultAsync(u => u.Code == DefaultUniversityCode);
            if (existing != null)
            {
                return existing;
            }

            var university = new University(
                Guid.NewGuid(),
                "Default University",
                DefaultUniversityCode,
                "Default seeded university",
                "123 College Ave",
                "info@defaultuniversity.edu",
                "+1-555-0100");

            await _universityRepository.InsertAsync(university, autoSave: true);
            _logger.LogInformation("Seeded default university {UniversityCode}", DefaultUniversityCode);
            return university;
        }

        private async Task<Faculty> EnsureDefaultFacultyAsync(Guid universityId)
        {
            var existing = await _facultyRepository.FirstOrDefaultAsync(f => f.Code == DefaultFacultyCode);
            if (existing != null)
            {
                return existing;
            }

            var faculty = new Faculty(
                Guid.NewGuid(),
                "Faculty of Computer Science",
                DefaultFacultyCode,
                universityId,
                "Faculty of Computer Science and Information Technology");

            await _facultyRepository.InsertAsync(faculty, autoSave: true);
            _logger.LogInformation("Seeded default faculty {FacultyCode}", DefaultFacultyCode);
            return faculty;
        }

        private async Task SeedProgramsAsync(Faculty faculty)
        {
            if (await _programRepository.GetCountAsync() > 0)
            {
                return;
            }

            var program1 = new Program(
                Guid.NewGuid(),
                "Computer Science",
                "CS",
                "Bachelor",
                8,
                120,
                faculty.Id,
                "Bachelor of Science in Computer Science");

            var program2 = new Program(
                Guid.NewGuid(),
                "Information Technology",
                "IT",
                "Bachelor",
                8,
                120,
                faculty.Id,
                "Bachelor of Science in Information Technology");

            await _programRepository.InsertAsync(program1, autoSave: true);
            await _programRepository.InsertAsync(program2, autoSave: true);
            _logger.LogInformation("Seeded default programs");
        }

        private async Task SeedCoursesAsync()
        {
            if (await _courseRepository.GetCountAsync() > 0)
            {
                return;
            }

            var programs = await _programRepository.GetListAsync();
            if (programs.Count == 0)
            {
                throw new BusinessException("Programs must be seeded before courses.");
            }

            var cs = programs.FirstOrDefault(p => p.Code == "CS");
            var it = programs.FirstOrDefault(p => p.Code == "IT");
            if (cs == null || it == null)
            {
                throw new BusinessException("Default CS/IT programs could not be found.");
            }

            var faculties = await _facultyRepository.GetListAsync();
            var fcs = faculties.FirstOrDefault(f => f.Code == DefaultFacultyCode);
            if (fcs == null)
            {
                throw new BusinessException($"Faculty '{DefaultFacultyCode}' could not be found while seeding courses.");
            }

            var course1 = new Course(
                Guid.NewGuid(),
                "Introduction to Programming",
                "CS101",
                3,
                1,
                2024,
                cs.Id,
                fcs.Id,
                "Basic programming concepts and problem-solving");

            var course2 = new Course(
                Guid.NewGuid(),
                "Data Structures and Algorithms",
                "CS102",
                4,
                2,
                2024,
                cs.Id,
                fcs.Id,
                "Fundamental data structures and algorithms");

            var course3 = new Course(
                Guid.NewGuid(),
                "Introduction to Information Technology",
                "IT101",
                3,
                1,
                2024,
                it.Id,
                fcs.Id,
                "Overview of IT concepts and applications");

            await _courseRepository.InsertAsync(course1, autoSave: true);
            await _courseRepository.InsertAsync(course2, autoSave: true);
            await _courseRepository.InsertAsync(course3, autoSave: true);
            _logger.LogInformation("Seeded default courses");
        }

        private async Task SeedStudentsAsync()
        {
            if (await _studentRepository.GetCountAsync() > 0)
            {
                return;
            }

            var programs = await _programRepository.GetListAsync();
            if (programs.Count == 0)
            {
                _logger.LogWarning("Skipping student seed because programs were not found.");
                return;
            }

            var random = new Random();

            for (var i = 0; i < 10; i++)
            {
                var program = programs[random.Next(programs.Count)];
                var student = Student.Create(
                    $"First{i}",
                    $"Last{i}",
                    $"student{i}@example.com",
                    DateTime.Now.AddYears(-20 - i),
                    i % 2 == 0 ? "Male" : "Female",
                    program.Id,
                    $"REF{i:000}", $"+1-555-01{i:00}");

                await _studentRepository.InsertAsync(student, autoSave: true);
            }

            _logger.LogInformation("Seeded demo students");
        }
    }
}
