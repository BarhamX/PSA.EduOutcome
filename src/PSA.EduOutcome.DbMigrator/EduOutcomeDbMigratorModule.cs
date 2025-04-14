using PSA.EduOutcome.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace PSA.EduOutcome.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EduOutcomeEntityFrameworkCoreModule),
    typeof(EduOutcomeApplicationContractsModule)
)]
public class EduOutcomeDbMigratorModule : AbpModule
{
}
