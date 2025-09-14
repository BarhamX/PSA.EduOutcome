using System;
using PSA.EduOutcome.Domain.Events;
using PSA.EduOutcome.Entities;
using Shouldly;
using Xunit;

namespace PSA.EduOutcome.Samples;

public class LearningOutcomeEventTests : EduOutcomeDomainTestBase<EduOutcomeDomainTestModule>
{
    [Fact]
    public void Should_Add_LearningOutcome_To_Course()
    {
        var course = new Course(Guid.NewGuid(), "Test", "C100", 3, 1, 2024, Guid.NewGuid(), Guid.NewGuid(), "Desc");
        var lo = new LearningOutcome(Guid.NewGuid(), "LO1", "desc", 50m, course.Id, "Knowledge", 1);

        course.AddLearningOutcome(lo);

        course.LearningOutcomes.ShouldContain(lo);
        course.LearningOutcomes.Count.ShouldBe(1);
    }
}
