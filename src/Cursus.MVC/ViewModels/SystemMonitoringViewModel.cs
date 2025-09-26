using Cursus.MVC.Models;

namespace Cursus.MVC.ViewModels
{
    public class SystemMonitoringViewModel
    {
        // Enrollment Statistics
        public int TotalEnrollments { get; set; }
        public int ActiveEnrollments { get; set; }
        public int CompletedEnrollments { get; set; }
        
        // Course Statistics
        public int TotalCourses { get; set; }
        public int ActiveCourses { get; set; }
        public int PendingCourses { get; set; }
        
        // User Statistics
        public int TotalUsers { get; set; }
        public int ActiveInstructors { get; set; }
        public int ActiveStudents { get; set; }
        
        // Recent Activity
        public List<EnrollViewModel> RecentEnrollments { get; set; } = new List<EnrollViewModel>();
        
        // Analytics Data
        public List<CourseCompletionViewModel> CourseCompletionData { get; set; } = new List<CourseCompletionViewModel>();
        public List<MonthlyEnrollmentViewModel> MonthlyEnrollmentData { get; set; } = new List<MonthlyEnrollmentViewModel>();
        
        // Calculated Properties
        public double EnrollmentCompletionRate => TotalEnrollments > 0 ? (double)CompletedEnrollments / TotalEnrollments * 100 : 0;
        public double CourseActiveRate => TotalCourses > 0 ? (double)ActiveCourses / TotalCourses * 100 : 0;
    }

    public class CourseCompletionViewModel
    {
        public string CourseName { get; set; } = string.Empty;
        public int TotalEnrollments { get; set; }
        public int CompletedEnrollments { get; set; }
        public double CompletionRate { get; set; }
    }

    public class MonthlyEnrollmentViewModel
    {
        public string Month { get; set; } = string.Empty;
        public int EnrollmentCount { get; set; }
    }
}