using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Discount
    {
        public Discount()
        {
            Courses = new HashSet<Course>();
        }

        [Key]
        public string DiscountId { get; set; }
        public string IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DiscountPercent { get; set; }

        // Navigation Property - Many-to-many with Course
        public virtual ICollection<Course> Courses { get; set; }
    }
}
