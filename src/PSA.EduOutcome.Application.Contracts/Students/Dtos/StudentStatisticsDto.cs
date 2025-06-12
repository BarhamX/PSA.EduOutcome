using System;

namespace PSA.EduOutcome.Students.Dtos
{
    public class StudentStatisticsDto
    {
        public int TotalCourses { get; set; }
        public int CompletedCourses { get; set; }
        public decimal AverageGrade { get; set; }
        public int LearningOutcomesAchieved { get; set; }
    }
}
