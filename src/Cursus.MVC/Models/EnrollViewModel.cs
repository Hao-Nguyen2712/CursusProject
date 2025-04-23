using Cursus.Domain.Models;

namespace Cursus.MVC.Models
{
    public class EnrollViewModel
    {
        public int EnrollId { get; set; }
        public int? CourseId { get; set; }
        public int? AccountId { get; set; }
        public string EnrollFinish { get; set; }
        public DateTime EnrollDate { get; set; }
        public string EnrollBlock { get; set; }
        public string EnrollStatus { get; set; }
        public virtual AccountViewModel Account { get; set; }
        public virtual CourseViewModel Course { get; set; }
    }
}
