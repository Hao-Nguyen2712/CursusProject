using Cursus.Domain.Models;

namespace Cursus.MVC.Models
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryStatus { get; set; }
        public virtual ICollection<CourseViewModel> CoursesVM { get; set; }
    }
}
