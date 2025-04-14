using Volo.Abp.Modularity;

namespace PSA.EduOutcome;

[DependsOn(
    typeof(EduOutcomeApplicationModule),
    typeof(EduOutcomeDomainTestModule)
)]
public class EduOutcomeApplicationTestModule : AbpModule
{

}
