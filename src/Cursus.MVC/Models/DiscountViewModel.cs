using Cursus.Domain.Models;

namespace Cursus.MVC.Models
{
    public class DiscountViewModel
    {
        public string DiscountId { get; set; }
        public string IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DiscountPercent { get; set; }

        public virtual ICollection<CourseViewModel> CoursesVM { get; set; }
    }
}
