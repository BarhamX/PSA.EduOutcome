using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSA.EduOutcome.EntityFrameworkCore;
using PSA.EduOutcome.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PSA.EduOutcome.Courses
{
    public class CourseRepository : 
        EfCoreRepository<EduOutcomeDbContext, Course, Guid>
        // ICourseRepository - Commented out since using standard ABP repositories
    {
        public CourseRepository(IDbContextProvider<EduOutcomeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Course> GetByCodeAsync(string code)
        {
            return await (await GetQueryableAsync())
                .FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<List<Course>> GetListByProgramAsync(Guid programId)
        {
            return await (await GetQueryableAsync())
                .Where(c => c.ProgramId == programId)
                .ToListAsync();
        }

        public async Task<List<Course>> GetListByFacultyAsync(Guid facultyId)
        {
            return await (await GetQueryableAsync())
                .Where(c => c.FacultyId == facultyId)
                .ToListAsync();
        }

        public async Task<Course> GetWithDetailsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Courses
                .Include(c => c.Program)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .Include(c => c.LearningOutcomes)
                .Include(c => c.Assessments)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Course>> GetPrerequisitesAsync(Course course)
        {
            // TODO: Implement prerequisite logic when prerequisite relationships are added to Course entity
            return new List<Course>();
        }

        public async Task<List<Course>> GetElectiveCoursesAsync(Guid programId, string semester)
        {
            // Convert string semester to int for comparison
            if (!int.TryParse(semester, out var semesterInt))
                semesterInt = 1; // Default to semester 1
                
            return await (await GetQueryableAsync())
                .Where(c => c.ProgramId == programId &&
                           c.Semester == semesterInt &&
                           c.IsElective &&
                           c.Status == "Active")
                .ToListAsync();
        }

        public override async Task<IQueryable<Course>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(c => c.Program)
                .Include(c => c.Enrollments)
                .Include(c => c.LearningOutcomes)
                .Include(c => c.Assessments);
        }
    }
} 