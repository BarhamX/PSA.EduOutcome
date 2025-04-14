using Volo.Abp.Settings;

namespace PSA.EduOutcome.Settings;

public class EduOutcomeSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(EduOutcomeSettings.MySetting1));
    }
}
