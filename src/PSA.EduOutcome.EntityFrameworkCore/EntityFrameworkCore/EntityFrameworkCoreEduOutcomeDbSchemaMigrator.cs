using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PSA.EduOutcome.Data;
using Volo.Abp.DependencyInjection;

namespace PSA.EduOutcome.EntityFrameworkCore;

public class EntityFrameworkCoreEduOutcomeDbSchemaMigrator
    : IEduOutcomeDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreEduOutcomeDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the EduOutcomeDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<EduOutcomeDbContext>()
            .Database
            .MigrateAsync();
    }
}
