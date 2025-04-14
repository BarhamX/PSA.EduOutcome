using PSA.EduOutcome.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace PSA.EduOutcome.Permissions;

public class EduOutcomePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(EduOutcomePermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(EduOutcomePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EduOutcomeResource>(name);
    }
}
