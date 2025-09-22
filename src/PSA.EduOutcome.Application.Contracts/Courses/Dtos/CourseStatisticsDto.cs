using System;
using System.Collections.Generic;

namespace PSA.EduOutcome.Courses.Dtos
{
    public class CourseStatisticsDto
    {
        public int TotalEnrollments { get; set; }
        public int ActiveEnrollments { get; set; }
        public decimal AverageGrade { get; set; }
        public int LearningOutcomesCovered { get; set; }
        public decimal AverageOutcomeAchievement { get; set; }
        public List<CourseLearningOutcomeAchievementDto> LearningOutcomeAchievements { get; set; } = new();
    }

    public class CourseLearningOutcomeAchievementDto
    {
        public Guid LearningOutcomeId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal AchievementPercentage { get; set; }
    }
}
