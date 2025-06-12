using System;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Events;

namespace EduOutcome.Domain.Events;

// This event is raised when a new LearningOutcome is added to a Course.
public class LearningOutcomeAddedEvent : EventData
{
    public Guid CourseId { get; }
    public Guid LearningOutcomeId { get; }

    public LearningOutcomeAddedEvent(Guid courseId, Guid learningOutcomeId)
    {
        CourseId = courseId;
        LearningOutcomeId = learningOutcomeId;
    }
}
