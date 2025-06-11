using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using PSA.EduOutcome.Localization;

namespace PSA.EduOutcome.Permissions
{
    public class EduOutcomePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var eduOutcomeGroup = context.AddGroup(EduOutcomePermissions.GroupName);

            var studentsPermission = eduOutcomeGroup.AddPermission(
                EduOutcomePermissions.Students.Default,
                L("Permission:Students"));

            studentsPermission.AddChild(
                EduOutcomePermissions.Students.Create,
                L("Permission:Students.Create"));

            studentsPermission.AddChild(
                EduOutcomePermissions.Students.Edit,
                L("Permission:Students.Edit"));

            studentsPermission.AddChild(
                EduOutcomePermissions.Students.Delete,
                L("Permission:Students.Delete"));

            var coursesPermission = eduOutcomeGroup.AddPermission(
                EduOutcomePermissions.Courses.Default,
                L("Permission:Courses"));

            coursesPermission.AddChild(
                EduOutcomePermissions.Courses.Create,
                L("Permission:Courses.Create"));

            coursesPermission.AddChild(
                EduOutcomePermissions.Courses.Edit,
                L("Permission:Courses.Edit"));

            coursesPermission.AddChild(
                EduOutcomePermissions.Courses.Delete,
                L("Permission:Courses.Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<EduOutcomeResource>(name);
        }
    }

    public static class EduOutcomePermissions
    {
        public const string GroupName = "EduOutcome";

        public static class Students
        {
            public const string Default = GroupName + ".Students";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class Courses
        {
            public const string Default = GroupName + ".Courses";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";
    }
}
