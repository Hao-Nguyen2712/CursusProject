using Cursus.Domain.Models;

namespace Cursus.MVC.Models
{
    public class CartViewModel
    {
        public int CartId { get; set; }
        public int? CourseId { get; set; }
        public int? AccountId { get; set; }
        public decimal? CartMoney { get; set; }

        public virtual AccountViewModel AccountVM { get; set; }
        public virtual CourseViewModel CourseVM { get; set; }
    }
}
