using System.Threading.Tasks;

namespace PSA.EduOutcome.Data;

public interface IEduOutcomeDbSchemaMigrator
{
    Task MigrateAsync();
}
