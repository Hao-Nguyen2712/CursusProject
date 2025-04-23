using Cursus.Domain.Models;

namespace Cursus.MVC.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public int? AccountId { get; set; }
        public string CourseName { get; set; }
        public string CourseShortDes { get; set; }
        public string CourseDescription { get; set; }
        public string CourseWlearn { get; set; }
        public string CourseRequirement { get; set; }
        public string CourseAvatar { get; set; }
        public DateTime? CourseDate { get; set; }
        public decimal? CourseMoney { get; set; }
        public string CourseStatus { get; set; }
        public int? CourseProcess { get; set; }
        public decimal? Discount { get; set; }
        public int? CategoryId { get; set; }

        public virtual AccountViewModel AccountVM { get; set; }
        public virtual CategoryViewModel CategoryVM { get; set; }
        public virtual DiscountViewModel DiscountVM { get; set; }
        public virtual ICollection<CartViewModel> CartsVM { get; set; }
        public virtual ICollection<EnrollViewModel> EnrollVM { get; set; }
        public virtual ICollection<LessonViewModel> LessonVM { get; set; }
        public virtual ICollection<RateViewModel> RateVM { get; set; }
    }
}
