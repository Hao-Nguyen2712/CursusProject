using System.ComponentModel.DataAnnotations;

namespace Cursus.Domain.Models
{
    public class CourseTop
    {
        [Key]
        public int CourseID { get; set; }
        public int CourseCount { get; set; }
    }
}
