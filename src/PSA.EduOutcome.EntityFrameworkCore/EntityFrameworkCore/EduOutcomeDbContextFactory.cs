using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PSA.EduOutcome.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class EduOutcomeDbContextFactory : IDesignTimeDbContextFactory<EduOutcomeDbContext>
{
    public EduOutcomeDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        EduOutcomeEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<EduOutcomeDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new EduOutcomeDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PSA.EduOutcome.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
