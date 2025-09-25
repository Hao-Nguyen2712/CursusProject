using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Category
    {
        public Category()
        {
            Courses = new HashSet<Course>();
        }

        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryStatus { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
