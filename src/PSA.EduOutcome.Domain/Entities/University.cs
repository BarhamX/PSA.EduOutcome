using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace PSA.EduOutcome.Entities
{
    public class University : Entity<Guid>
    {
        public string Name { get; private set; }
        public List<Faculty> Faculties { get; private set; }

        public University(Guid id, string name) : base(id)
        {
            Name = name;
            Faculties = new List<Faculty>();
        }

        public void AddFaculty(Faculty faculty)
        {
            Faculties.Add(faculty);
        }
    }
}
