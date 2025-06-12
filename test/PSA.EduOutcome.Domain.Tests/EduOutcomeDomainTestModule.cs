using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace PSA.EduOutcome;

[DependsOn(
    typeof(EduOutcomeDomainModule),
    typeof(EduOutcomeTestBaseModule)
)]
public class EduOutcomeDomainTestModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        // Skip default test data seeding for lightweight unit tests
    }

}
