using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.MVC.Models;

namespace Cursus.MVC.ViewModels
{
    public class HomePageViewViewModel
    {
        public AccountViewModel UserAcc { get; set; }
        public int CountCourse { get; set; }
        public int CountSub { get; set; }
        public List<AccountViewModel> SubscribeInstructor { set; get; }
        public List<CourseViewModel> EnrollCourse { set; get; }
        public List<Course>? EnrolledCourse {set; get;}
        public List<Course>? PuchasedCourse {set; get;}
        public List<CourseViewModel> CourseList { set; get; }
        public List<AccountViewModel> InstructorsList { set; get; }
        public Dictionary<int, double>? CourseRatings { set; get; }
    }
}