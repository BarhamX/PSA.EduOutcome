using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using PSA.EduOutcome.Entities;

namespace PSA.EduOutcome.Entities
{
    public class LearningOutcome : AggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int TotalMarks { get; private set; }
        public Guid CourseId { get; private set; }

        private readonly List<Question> _questions;
        public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();

        public LearningOutcome(Guid id, string name, string description, int totalMarks, Guid courseId)
            : base(id)
        {
            Name = name;
            Description = description;
            TotalMarks = totalMarks;
            CourseId = courseId;
            _questions = new List<Question>();
        }

        public void AddQuestion(Question question)
        {
            var sumMarks = _questions.Sum(q => q.Marks) + question.Marks;
            if (sumMarks > TotalMarks)
            {
                throw new BusinessException("Adding this question exceeds the total marks allocated for the learning outcome.");
            }
            _questions.Add(question);
            // You might consider adding a domain event here for a question added, if other parts of your domain need to react.
        }
    }
}
