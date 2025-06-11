using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSA.EduOutcome.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PSA.EduOutcome.Courses
{
    public class CourseRepository : 
        EfCoreRepository<EduOutcomeDbContext, Course, Guid>,
        ICourseRepository
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
                .Include(c => c.Faculty)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .Include(c => c.Questions)
                    .ThenInclude(q => q.LearningOutcomes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Course>> GetPrerequisitesAsync(Course course)
        {
            if (course.PrerequisiteCourseId == null)
            {
                return new List<Course>();
            }

            var dbContext = await GetDbContextAsync();
            return await dbContext.Courses
                .Where(c => c.Id == course.PrerequisiteCourseId)
                .ToListAsync();
        }

        public async Task<List<Course>> GetElectiveCoursesAsync(Guid programId, int semester)
        {
            return await (await GetQueryableAsync())
                .Where(c => c.ProgramId == programId &&
                           c.Semester == semester &&
                           c.IsElective &&
                           c.Status == "Active")
                .ToListAsync();
        }

        public override async Task<IQueryable<Course>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(c => c.Program)
                .Include(c => c.Faculty)
                .Include(c => c.Enrollments)
                .Include(c => c.Questions);
        }
    }
} 