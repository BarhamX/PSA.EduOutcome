using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
//using Volo.Abp.Domain.Events;
using Volo.Abp.EventBus;

namespace EduOutcome.Domain.Events
{
    // This event is raised when a new LearningOutcome is added to a Course.
    public class LearningOutcomeAddedEvent //: DomainEvent
    {
        public Guid CourseId { get; }
        public Guid LearningOutcomeId { get; }

        public LearningOutcomeAddedEvent(Guid courseId, Guid learningOutcomeId)
        {
            CourseId = courseId;
            LearningOutcomeId = learningOutcomeId;
        }
    }
}
