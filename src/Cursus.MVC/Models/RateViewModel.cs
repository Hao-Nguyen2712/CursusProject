using Cursus.Domain.Models;

namespace Cursus.MVC.Models
{
    public class RateViewModel
    {
        public int RateId { get; set; }
        public int? CourseId { get; set; }
        public int? AccountId { get; set; }
        public int? RatePoint { get; set; }
        public string? RateContent { get; set; }
        public DateTime? RateDate { get; set; } = DateTime.Now;
        public virtual AccountViewModel? AccountVM { get; set; }
        public virtual CourseViewModel? CourseVM { get; set; }
    }
}
