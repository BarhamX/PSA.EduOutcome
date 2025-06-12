using System;
using PSA.EduOutcome.Entities;
using Shouldly;
using Xunit;

namespace PSA.EduOutcome.Samples;

public class AssessmentTests : EduOutcomeDomainTestBase<EduOutcomeDomainTestModule>
{
    [Fact]
    public void Should_Apply_Late_Penalty_When_Submitted_After_DueDate()
    {
        var assessment = new Assessment(
            Guid.NewGuid(),
            "Midterm",
            AssessmentType.Exam,
            100m,
            0.4m,
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(-1),
            "Midterm exam");

        assessment.ConfigureLateSubmission(true, 10);

        var result = assessment.CalculateFinalMark(80m, DateTime.UtcNow);

        result.ShouldBe(72m);
    }
}
