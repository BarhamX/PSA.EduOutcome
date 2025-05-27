using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PSA.EduOutcome.Entities
{
    public class University : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public string Address { get; private set; }
        public string ContactEmail { get; private set; }
        public string ContactPhone { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation properties
        public virtual ICollection<Faculty> Faculties { get; private set; }

        protected University()
        {
            // Required for EF Core
            Faculties = new HashSet<Faculty>();
        }

        public University(
            Guid id,
            string name,
            string code,
            string description = null,
            string address = null,
            string contactEmail = null,
            string contactPhone = null) : base(id)
        {
            SetName(name);
            SetCode(code);
            Description = description;
            Address = address;
            ContactEmail = contactEmail;
            ContactPhone = contactPhone;
            IsActive = true;
            Faculties = new HashSet<Faculty>();
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 200);
        }

        public void SetCode(string code)
        {
            Code = Check.NotNullOrWhiteSpace(code, nameof(code), maxLength: 50);
        }

        public void SetContactInfo(string email, string phone, string address)
        {
            ContactEmail = email;
            ContactPhone = phone;
            Address = address;
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
