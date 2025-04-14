using Volo.Abp.Modularity;

namespace PSA.EduOutcome;

[DependsOn(
    typeof(EduOutcomeDomainModule),
    typeof(EduOutcomeTestBaseModule)
)]
public class EduOutcomeDomainTestModule : AbpModule
{

}
