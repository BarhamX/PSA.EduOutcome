using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
//using PSA.EduOutcome.Domain.Events;

namespace PSA.EduOutcome.Entities
{
    public class Course : AggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public Guid ProgramId { get; private set; }

        private readonly List<LearningOutcome> _learningOutcomes;
        public IReadOnlyCollection<LearningOutcome> LearningOutcomes => _learningOutcomes.AsReadOnly();

        private readonly List<Assessment> _assessments;
        public IReadOnlyCollection<Assessment> Assessments => _assessments.AsReadOnly();

        public Course(Guid id, string name, Guid programId)
            : base(id)
        {
            Name = name;
            ProgramId = programId;
            _learningOutcomes = new List<LearningOutcome>();
            _assessments = new List<Assessment>();
        }

        public void AddLearningOutcome(LearningOutcome learningOutcome)
        {
            if (_learningOutcomes.Sum(lo => lo.TotalMarks) + learningOutcome.TotalMarks > 100)
            {
                throw new BusinessException("The total marks for learning outcomes exceed the course's limit.");
            }
            _learningOutcomes.Add(learningOutcome);
            // Publish domain event for a new learning outcome added.
      //      AddDomainEvent(new LearningOutcomeAddedEvent(this.Id, learningOutcome.Id));
        }

        public void AddAssessment(Assessment assessment)
        {
            if (assessment.TotalMarks != assessment.Questions.Sum(q => q.Marks))
            {
                throw new BusinessException("The total marks of questions in an assessment do not match the assessment's total.");
            }
            _assessments.Add(assessment);
            // Optionally, publish a different domain event here if needed.
        }
    }
}
