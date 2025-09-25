#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Cursus.Domain.Models
{
    public partial class Enroll
    {
        [Key]
        public int EnrollId { get; set; }
        public int? CourseId { get; set; }
        public int? AccountId { get; set; }
        public string EnrollFinish { get; set; }
        public DateTime? EnrollDate { get; set; }
        public string EnrollBlock { get; set; }
        public string EnrollStatus { get; set; }

        public virtual Account Account { get; set; }
        public virtual Course Course { get; set; }
    }
}
