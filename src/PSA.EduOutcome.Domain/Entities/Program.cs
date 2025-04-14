using System;
using Volo.Abp.Domain.Entities;

namespace PSA.EduOutcome.Entities
{
    public class Program : Entity<Guid>
    {
        public string Name { get; private set; }
        public Guid FacultyId { get; private set; }

        public Program(Guid id, string name, Guid facultyId) : base(id)
        {
            Name = name;
            FacultyId = facultyId;
        }
    }
}
