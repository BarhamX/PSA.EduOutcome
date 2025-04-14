using PSA.EduOutcome.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PSA.EduOutcome.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class EduOutcomeController : AbpControllerBase
{
    protected EduOutcomeController()
    {
        LocalizationResource = typeof(EduOutcomeResource);
    }
}
