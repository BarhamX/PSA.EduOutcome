using Volo.Abp.Modularity;

namespace PSA.EduOutcome;

/* Inherit from this class for your domain layer tests. */
public abstract class EduOutcomeDomainTestBase<TStartupModule> : EduOutcomeTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
