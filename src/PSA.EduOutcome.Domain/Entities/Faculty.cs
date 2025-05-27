using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PSA.EduOutcome.Entities
{
    public class Faculty : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public Guid UniversityId { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation properties
        public virtual University University { get; private set; }
        public virtual ICollection<Program> Programs { get; private set; }

        protected Faculty()
        {
            // Required for EF Core
            Programs = new HashSet<Program>();
        }

        public Faculty(
            Guid id,
            string name,
            string code,
            Guid universityId,
            string description = null) : base(id)
        {
            SetName(name);
            SetCode(code);
            UniversityId = universityId;
            Description = description;
            IsActive = true;
            Programs = new HashSet<Program>();
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 200);
        }

        public void SetCode(string code)
        {
            Code = Check.NotNullOrWhiteSpace(code, nameof(code), maxLength: 50);
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
