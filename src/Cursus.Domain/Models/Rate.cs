#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Cursus.Domain.Models
{
    public partial class Rate
    {
        [Key]
        public int RateId { get; set; }
        public int? CourseId { get; set; }
        public int? AccountId { get; set; }
        public int? RatePoint { get; set; }
        public string RateContent { get; set; }
        public DateTime? RateDate { get; set; }

        public virtual Account Account { get; set; }
        public virtual Course Course { get; set; }
    }
}
