using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;
namespace Cursus.MVC.ViewModels
{
    public class ProFileViewViewModel
    {
        public Account? UserAcc { get; set; }
        public List<Course>? EnrollCourse { set; get; }
        public List<Course>? PurchasedCourse { set; get; }
        public List<Course>? CourseList { set; get; }
        public List<Account>? InstructorsList { set; get; }
        public List<Account>? SubscribeInstructor { set; get; }

    }
}