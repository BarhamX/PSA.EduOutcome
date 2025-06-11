using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PSA.EduOutcome.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace PSA.EduOutcome.Data
{
    public class EduOutcomeDataSeeder : IDataSeeder, ITransientDependency
    {
        private readonly ILogger<EduOutcomeDataSeeder> _logger;
        private readonly IRepository<Student, Guid> _studentRepository;
        private readonly IRepository<Program, Guid> _programRepository;
        private readonly IRepository<Course, Guid> _courseRepository;
        private readonly IRepository<Faculty, Guid> _facultyRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public EduOutcomeDataSeeder(
            ILogger<EduOutcomeDataSeeder> logger,
            IRepository<Student, Guid> studentRepository,
            IRepository<Program, Guid> programRepository,
            IRepository<Course, Guid> courseRepository,
            IRepository<Faculty, Guid> facultyRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _logger = logger;
            _studentRepository = studentRepository;
            _programRepository = programRepository;
            _courseRepository = courseRepository;
            _facultyRepository = facultyRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (context["DataSeed:IsSeeded"] == "true")
            {
                return;
            }

            await SeedFacultiesAsync();
            await SeedProgramsAsync();
            await SeedCoursesAsync();
            await SeedStudentsAsync();

            context["DataSeed:IsSeeded"] = "true";
        }

        private async Task SeedFacultiesAsync()
        {
            if (await _facultyRepository.GetCountAsync() > 0)
            {
                return;
            }

            var faculties = new[]
            {
                new Faculty
                {
                    Name = "Faculty of Computer Science",
                    Code = "FCS",
                    Description = "Faculty of Computer Science and Information Technology"
                },
                new Faculty
                {
                    Name = "Faculty of Engineering",
                    Code = "FE",
                    Description = "Faculty of Engineering and Technology"
                }
            };

            foreach (var faculty in faculties)
            {
                await _facultyRepository.InsertAsync(faculty);
            }

            _logger.LogInformation("Seeded faculties");
        }

        private async Task SeedProgramsAsync()
        {
            if (await _programRepository.GetCountAsync() > 0)
            {
                return;
            }

            var faculties = await _facultyRepository.GetListAsync();
            var fcs = faculties.First(f => f.Code == "FCS");

            var programs = new[]
            {
                new Program
                {
                    Name = "Computer Science",
                    Code = "CS",
                    Description = "Bachelor of Science in Computer Science",
                    FacultyId = fcs.Id
                },
                new Program
                {
                    Name = "Information Technology",
                    Code = "IT",
                    Description = "Bachelor of Science in Information Technology",
                    FacultyId = fcs.Id
                }
            };

            foreach (var program in programs)
            {
                await _programRepository.InsertAsync(program);
            }

            _logger.LogInformation("Seeded programs");
        }

        private async Task SeedCoursesAsync()
        {
            if (await _courseRepository.GetCountAsync() > 0)
            {
                return;
            }

            var programs = await _programRepository.GetListAsync();
            var cs = programs.First(p => p.Code == "CS");
            var it = programs.First(p => p.Code == "IT");
            var faculties = await _facultyRepository.GetListAsync();
            var fcs = faculties.First(f => f.Code == "FCS");

            var courses = new[]
            {
                new Course
                {
                    Code = "CS101",
                    Name = "Introduction to Programming",
                    Description = "Basic programming concepts and problem-solving",
                    Credits = 3,
                    ProgramId = cs.Id,
                    FacultyId = fcs.Id,
                    Status = "Active",
                    IsElective = false,
                    Semester = 1,
                    AcademicYear = 1
                },
                new Course
                {
                    Code = "CS102",
                    Name = "Data Structures and Algorithms",
                    Description = "Fundamental data structures and algorithms",
                    Credits = 4,
                    ProgramId = cs.Id,
                    FacultyId = fcs.Id,
                    Status = "Active",
                    IsElective = false,
                    PrerequisiteCourseId = 1, // CS101
                    Semester = 2,
                    AcademicYear = 1
                },
                new Course
                {
                    Code = "IT101",
                    Name = "Introduction to Information Technology",
                    Description = "Overview of IT concepts and applications",
                    Credits = 3,
                    ProgramId = it.Id,
                    FacultyId = fcs.Id,
                    Status = "Active",
                    IsElective = false,
                    Semester = 1,
                    AcademicYear = 1
                }
            };

            foreach (var course in courses)
            {
                await _courseRepository.InsertAsync(course);
            }

            _logger.LogInformation("Seeded courses");
        }

        private async Task SeedStudentsAsync()
        {
            if (await _studentRepository.GetCountAsync() > 0)
            {
                return;
            }

            var programs = await _programRepository.GetListAsync();
            var random = new Random();

            for (int i = 0; i < 10; i++)
            {
                var program = programs[random.Next(programs.Count)];
                var student = Student.Create(
                    $"First{i}",
                    $"Last{i}",
                    $"student{i}@example.com",
                    DateTime.Now.AddYears(-20 - i),
                    i % 2 == 0 ? "Male" : "Female",
                    program.Id,
                    $"REF{i:000}"
                );

                await _studentRepository.InsertAsync(student);
            }

            _logger.LogInformation("Seeded students");
        }
    }
} 