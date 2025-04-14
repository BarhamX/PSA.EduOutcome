using Volo.Abp.Modularity;

namespace PSA.EduOutcome;

public abstract class EduOutcomeApplicationTestBase<TStartupModule> : EduOutcomeTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
