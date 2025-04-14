using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace PSA.EduOutcome.Data;

/* This is used if database provider does't define
 * IEduOutcomeDbSchemaMigrator implementation.
 */
public class NullEduOutcomeDbSchemaMigrator : IEduOutcomeDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
