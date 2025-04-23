using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cursus.Domain.ViewModels
{
    public class DashBoard
    {
        public int TotalEnrolls { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalEnrolledUsers { get; set; }
        public int TotalCourses { get; set; }
        public int TotalSubscrise {get; set;}
        public List<Domain.Models.Course> CourseList { get; set; }
        public List<Domain.Models.Course> PendingCourse { get; set; }
        public List<Domain.Models.Course> RejectCourse { get; set; }
        public List<Domain.Models.Subscribe> SubscribesList { get; set; }
    }
}