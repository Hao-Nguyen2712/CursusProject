using Cursus.Domain.Models;

namespace Cursus.Domain.ViewModels
{
    public class HomePageView
    {
        public Account? UserAcc { get; set; }
        public List<Account>? SubscribeInstructor { set; get; }
        public List<Course>? EnrollCourse { set; get; }
        public List<Course>? PuchasedCourse { set; get; }
        public List<Course>? EnrolledCourse { set; get; }
        public List<Course>? CourseList { set; get; }
        public List<Account>? InstructorsList { set; get; }
        public Dictionary<int, double>? CourseRatings { set; get; }
    }
}