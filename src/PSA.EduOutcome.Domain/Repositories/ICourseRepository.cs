using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PSA.EduOutcome.Entities;
using Volo.Abp.Domain.Repositories;

namespace PSA.EduOutcome.Domain.Repositories
{
    public interface ICourseRepository : IRepository<Course, Guid>
    {
        Task<Course> GetWithDetailsAsync(Guid id);
        Task<List<Course>> GetCoursesByProgramAsync(Guid programId);
        Task<List<Course>> GetCoursesByInstructorAsync(Guid instructorId);
        Task<Course> FindByCodeAsync(string code);
        Task<List<Course>> GetElectiveCoursesAsync(Guid programId, string semester);
    }
}
