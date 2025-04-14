using PSA.EduOutcome.Entities;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace EduOutcome.Domain.Entities
{
    public class Student : AggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public Guid ProgramId { get; private set; }

        private readonly List<StudentGrade> _grades;
        public IReadOnlyCollection<StudentGrade> Grades => _grades.AsReadOnly();

        public Student(Guid id, string name, Guid programId)
            : base(id)
        {
            Name = name;
            ProgramId = programId;
            _grades = new List<StudentGrade>();
        }

        public void AddGrade(StudentGrade grade)
        {
            _grades.Add(grade);
            // If needed, publish an event like StudentGradeAddedEvent here.
        }
    }
}
