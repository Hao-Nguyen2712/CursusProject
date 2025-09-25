namespace Cursus.Domain.Models
{
    public class AdminDashBoard
    {
        public int TotalUsers { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalCourses { get; set; }
        public int PendingReports { get; set; }
        public List<Domain.Models.Course> PendingCourseList { get; set; }
        public List<Domain.Models.Account> IntructorsList { get; set; }
    }
}