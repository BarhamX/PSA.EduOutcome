
namespace PSA.EduOutcome.Permissions
{
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

        public static class LearningOutcomes
        {
            public const string Default = GroupName + ".LearningOutcomes";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class Assessments
        {
            public const string Default = GroupName + ".Assessments";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Publish = Default + ".Publish";
        }

        public static class Reports
        {
            public const string Default = GroupName + ".Reports";
            public const string Generate = Default + ".Generate";
            public const string Export = Default + ".Export";
        }
    }
}
