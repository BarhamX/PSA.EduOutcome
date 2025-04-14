using PSA.EduOutcome.Localization;
using Volo.Abp.Application.Services;

namespace PSA.EduOutcome;

/* Inherit your application services from this class.
 */
public abstract class EduOutcomeAppService : ApplicationService
{
    protected EduOutcomeAppService()
    {
        LocalizationResource = typeof(EduOutcomeResource);
    }
}
