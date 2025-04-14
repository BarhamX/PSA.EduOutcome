using Microsoft.Extensions.Localization;
using PSA.EduOutcome.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace PSA.EduOutcome;

[Dependency(ReplaceServices = true)]
public class EduOutcomeBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<EduOutcomeResource> _localizer;

    public EduOutcomeBrandingProvider(IStringLocalizer<EduOutcomeResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
