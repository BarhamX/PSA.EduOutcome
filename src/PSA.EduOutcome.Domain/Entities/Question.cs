using System;
using Volo.Abp.Domain.Entities;

namespace PSA.EduOutcome.Entities
{
    public class Question : Entity<Guid>
    {
        public string Text { get; private set; }
        public int Marks { get; private set; }
        public Guid LearningOutcomeId { get; private set; }
        public Guid AssessmentId { get; private set; }

        public Question(Guid id, string text, int marks, Guid learningOutcomeId, Guid assessmentId)
            : base(id)
        {
            Text = text;
            Marks = marks;
            LearningOutcomeId = learningOutcomeId;
            AssessmentId = assessmentId;
        }
    }
}
