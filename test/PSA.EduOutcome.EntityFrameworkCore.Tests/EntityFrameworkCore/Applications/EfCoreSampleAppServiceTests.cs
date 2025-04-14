using PSA.EduOutcome.Samples;
using Xunit;

namespace PSA.EduOutcome.EntityFrameworkCore.Applications;

[Collection(EduOutcomeTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<EduOutcomeEntityFrameworkCoreTestModule>
{

}
