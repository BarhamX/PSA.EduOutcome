using System;

namespace PSA.EduOutcome.Courses.Dtos
{
    public class CourseStatisticsDto
    {
        public int TotalEnrollments { get; set; }
        public int ActiveEnrollments { get; set; }
        public decimal AverageGrade { get; set; }
        public int LearningOutcomesCovered { get; set; }
    }
}
