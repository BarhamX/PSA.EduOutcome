using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PSA.EduOutcome.Entities;
using Volo.Abp.Domain.Repositories;

namespace PSA.EduOutcome.Domain.Repositories
{
    public interface IStudentResponseRepository : IRepository<StudentResponse, Guid>
    {
        Task<StudentResponse> FindByStudentAndQuestionAsync(Guid studentId, Guid questionId);
        Task<List<StudentResponse>> GetByStudentAndAssessmentAsync(Guid studentId, Guid assessmentId);
        Task<List<StudentResponse>> GetByEnrollmentAsync(Guid enrollmentId);
        Task<List<StudentResponse>> GetByQuestionAsync(Guid questionId);
    }
} 