using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PSA.EduOutcome.EntityFrameworkCore;
using PSA.EduOutcome.Entities;
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

            var university1 = Guid.NewGuid(); // Default university for seeding
            
            var faculty1 = new Faculty(
                Guid.NewGuid(),
                "Faculty of Computer Science",
                "FCS",
                university1,
                "Faculty of Computer Science and Information Technology"
            );
            
            var faculty2 = new Faculty(
                Guid.NewGuid(),
                "Faculty of Engineering",
                "FE",
                university1,
                "Faculty of Engineering and Technology"
            );
            
            var faculties = new[] { faculty1, faculty2 };

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

            var program1 = new Program(
                Guid.NewGuid(),
                "Computer Science",
                "CS",
                "Bachelor",
                8, // 4 years = 8 semesters
                120, // Total credits
                fcs.Id,
                "Bachelor of Science in Computer Science"
            );
            
            var program2 = new Program(
                Guid.NewGuid(),
                "Information Technology",
                "IT",
                "Bachelor",
                8, // 4 years = 8 semesters
                120, // Total credits
                fcs.Id,
                "Bachelor of Science in Information Technology"
            );
            
            var programs = new[] { program1, program2 };

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

            var course1 = new Course(
                Guid.NewGuid(),
                "Introduction to Programming",
                "CS101",
                3,
                1,
                2024,
                cs.Id,
                fcs.Id,
                "Basic programming concepts and problem-solving"
            );
            
            var course2 = new Course(
                Guid.NewGuid(),
                "Data Structures and Algorithms",
                "CS102",
                4,
                2,
                2024,
                cs.Id,
                fcs.Id,
                "Fundamental data structures and algorithms"
            );
            
            var course3 = new Course(
                Guid.NewGuid(),
                "Introduction to Information Technology",
                "IT101",
                3,
                1,
                2024,
                it.Id,
                fcs.Id,
                "Overview of IT concepts and applications"
            );
            
            var courses = new[] { course1, course2, course3 };

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