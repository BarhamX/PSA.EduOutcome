using System;
using EduOutcome.Domain.Events;
using PSA.EduOutcome.Entities;
using Shouldly;
using Xunit;

namespace PSA.EduOutcome.Samples;

public class LearningOutcomeEventTests : EduOutcomeDomainTestBase<EduOutcomeDomainTestModule>
{
    [Fact]
    public void Should_Add_Domain_Event_When_LearningOutcome_Added()
    {
        var course = new Course(Guid.NewGuid(), "Test", "C100", "Desc", 3, Guid.NewGuid(), Guid.NewGuid(), "Fall", 2024);
        var lo = new LearningOutcome(Guid.NewGuid(), "LO1", "desc", 50m, course.Id, "Knowledge");

        course.AddLearningOutcome(lo);

        course.DomainEvents.ShouldContain(e => e is LearningOutcomeAddedEvent);
    }
}
