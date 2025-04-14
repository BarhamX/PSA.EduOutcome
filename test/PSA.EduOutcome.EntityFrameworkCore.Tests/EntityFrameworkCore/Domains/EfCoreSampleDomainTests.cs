using PSA.EduOutcome.Samples;
using Xunit;

namespace PSA.EduOutcome.EntityFrameworkCore.Domains;

[Collection(EduOutcomeTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<EduOutcomeEntityFrameworkCoreTestModule>
{

}
