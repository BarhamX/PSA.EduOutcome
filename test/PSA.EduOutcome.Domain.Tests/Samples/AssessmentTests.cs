using System;
using PSA.EduOutcome.Entities;
using Shouldly;
using Xunit;

namespace PSA.EduOutcome.Samples;

public class AssessmentTests : EduOutcomeDomainTestBase<EduOutcomeDomainTestModule>
{
    [Fact]
    public void CalculateFinalMark_Should_Apply_LatePenalty()
    {
        var dueDate = DateTime.Now.AddDays(1);
        var assessment = new Assessment(
            Guid.NewGuid(),
            "Sample Assessment",
            AssessmentType.Exam,
            100m,
            20m,
            Guid.NewGuid(),
            dueDate);

        assessment.ConfigureLateSubmission(true, 10);

        var result = assessment.CalculateFinalMark(80m, dueDate.AddDays(1));

        result.ShouldBe(72m);
    }
}
