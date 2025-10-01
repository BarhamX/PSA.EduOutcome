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

            // Students permissions
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

            // Courses permissions
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

            // Learning Outcomes permissions
            var learningOutcomesPermission = eduOutcomeGroup.AddPermission(
                EduOutcomePermissions.LearningOutcomes.Default,
                L("Permission:LearningOutcomes"));

            learningOutcomesPermission.AddChild(
                EduOutcomePermissions.LearningOutcomes.Create,
                L("Permission:LearningOutcomes.Create"));

            learningOutcomesPermission.AddChild(
                EduOutcomePermissions.LearningOutcomes.Edit,
                L("Permission:LearningOutcomes.Edit"));

            learningOutcomesPermission.AddChild(
                EduOutcomePermissions.LearningOutcomes.Delete,
                L("Permission:LearningOutcomes.Delete"));

            // Question permissions
            var questionsPermission = eduOutcomeGroup.AddPermission(
                EduOutcomePermissions.Questions.Default,
                L("Permission:Questions"));

            questionsPermission.AddChild(
                EduOutcomePermissions.Questions.Create,
                L("Permission:Questions.Create"));

            questionsPermission.AddChild(
                EduOutcomePermissions.Questions.Edit,
                L("Permission:Questions.Edit"));

            questionsPermission.AddChild(
                EduOutcomePermissions.Questions.Delete,
                L("Permission:Questions.Delete"));
            // Assessments permissions
            var assessmentsPermission = eduOutcomeGroup.AddPermission(
                EduOutcomePermissions.Assessments.Default,
                L("Permission:Assessments"));

            assessmentsPermission.AddChild(
                EduOutcomePermissions.Assessments.Create,
                L("Permission:Assessments.Create"));

            assessmentsPermission.AddChild(
                EduOutcomePermissions.Assessments.Edit,
                L("Permission:Assessments.Edit"));

            assessmentsPermission.AddChild(
                EduOutcomePermissions.Assessments.Delete,
                L("Permission:Assessments.Delete"));

            assessmentsPermission.AddChild(
                EduOutcomePermissions.Assessments.Publish,
                L("Permission:Assessments.Publish"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<EduOutcomeResource>(name);
        }
    }
}
