using System;
using Volo.Abp.Domain.Entities;

namespace PSA.EduOutcome.Entities
{
    public class StudentGrade : Entity<Guid>
    {
        public Guid StudentId { get; private set; }
        public Guid QuestionId { get; private set; }
        public int Grade { get; private set; }

        public StudentGrade(Guid id, Guid studentId, Guid questionId, int grade)
            : base(id)
        {
            StudentId = studentId;
            QuestionId = questionId;
            Grade = grade;
        }
    }
}
