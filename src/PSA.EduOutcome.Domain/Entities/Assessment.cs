using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using PSA.EduOutcome.Entities;

namespace PSA.EduOutcome.Entities
{
    public class Assessment : AggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public DateTime AssessmentDate { get; private set; }
        public int TotalMarks { get; private set; }
        public Guid CourseId { get; private set; }

        private readonly List<Question> _questions;
        public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();

        public Assessment(Guid id, string name, DateTime assessmentDate, int totalMarks, Guid courseId)
            : base(id)
        {
            Name = name;
            AssessmentDate = assessmentDate;
            TotalMarks = totalMarks;
            CourseId = courseId;
            _questions = new List<Question>();
        }

        public void AddQuestion(Question question)
        {
            var sumMarks = _questions.Sum(q => q.Marks) + question.Marks;
            if (sumMarks > TotalMarks)
            {
                throw new BusinessException("The sum of marks for the questions exceeds the total marks of the assessment.");
            }
            _questions.Add(question);
            // Optionally, publish an event (e.g., AssessmentQuestionAddedEvent) if required.
        }
    }
}
