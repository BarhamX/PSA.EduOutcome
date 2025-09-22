using System;
using System.Collections.Generic;

namespace PSA.EduOutcome.Students.Dtos
{
    public class StudentStatisticsDto
    {
        public int TotalCourses { get; set; }
        public int CompletedCourses { get; set; }
        public int ActiveCourses { get; set; }
        public decimal AverageGrade { get; set; }
        public int LearningOutcomesAchieved { get; set; }
        public decimal AverageOutcomeAchievement { get; set; }
        public List<StudentCourseAchievementDto> CourseAchievements { get; set; } = new();
    }

    public class StudentCourseAchievementDto
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public decimal AchievementPercentage { get; set; }
        public List<StudentLearningOutcomeAchievementDto> LearningOutcomeAchievements { get; set; } = new();
    }

    public class StudentLearningOutcomeAchievementDto
    {
        public Guid LearningOutcomeId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal AchievementPercentage { get; set; }
    }
}
