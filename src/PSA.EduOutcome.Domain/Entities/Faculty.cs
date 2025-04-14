using System;
using Volo.Abp.Domain.Entities;

namespace PSA.EduOutcome.Entities
{
    public class Faculty : Entity<Guid>
    {
        public string Name { get; private set; }
        public Guid UniversityId { get; private set; }

        public Faculty(Guid id, string name, Guid universityId) : base(id)
        {
            Name = name;
            UniversityId = universityId;
        }
    }
}
