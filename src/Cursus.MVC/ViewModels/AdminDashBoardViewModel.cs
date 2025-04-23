using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cursus.MVC.ViewModels
{
    public class AdminDashBoardViewModel
    {
        public int TotalUsers { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalCourses { get; set; }
        public int PendingReports { get; set; }
        public List<Domain.Models.Course> PendingCourseList { get; set; }
        public List<Domain.Models.Account> IntructorsList { get; set; }
    }
}