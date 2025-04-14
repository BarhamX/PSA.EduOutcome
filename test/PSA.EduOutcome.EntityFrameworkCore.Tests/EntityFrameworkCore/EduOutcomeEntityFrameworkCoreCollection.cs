using Xunit;

namespace PSA.EduOutcome.EntityFrameworkCore;

[CollectionDefinition(EduOutcomeTestConsts.CollectionDefinitionName)]
public class EduOutcomeEntityFrameworkCoreCollection : ICollectionFixture<EduOutcomeEntityFrameworkCoreFixture>
{

}
