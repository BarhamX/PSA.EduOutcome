using System;
using Volo.Abp.Domain.Values;

namespace PSA.EduOutcome.ValueObjects
{
    public class Grade : AbpValueObject
    {
        public decimal Value { get; }

        public Grade(decimal value)
        {
            if (value < 0 || value > 100)
                throw new ArgumentOutOfRangeException(nameof(value), "Grade must be between 0 and 100.");
            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
} 