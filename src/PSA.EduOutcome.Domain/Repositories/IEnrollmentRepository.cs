using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PSA.EduOutcome.Entities;
using Volo.Abp.Domain.Repositories;

namespace PSA.EduOutcome.Domain.Repositories
{
    public interface IEnrollmentRepository : IRepository<Enrollment, Guid>
    {
        Task<Enrollment> FindByStudentAndCourseAsync(Guid studentId, Guid courseId);
        Task<List<Enrollment>> GetByStudentAsync(Guid studentId);
        Task<List<Enrollment>> GetByCourseAsync(Guid courseId);
        Task<List<Enrollment>> GetActiveEnrollmentsByCourseAsync(Guid courseId);
    }
} 