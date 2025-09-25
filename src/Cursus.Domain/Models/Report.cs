#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Cursus.Domain.Models
{
    public partial class Report
    {
        [Key]
        public int ReportId { get; set; }
        
        public int? CourseId { get; set; }
        public int? CmtId { get; set; }
        public int? AccountId { get; set; }
        
        public string ReportReasson { get; set; }
        public DateTime? ReportDate { get; set; }

        // Navigation Properties
        public virtual Course Course { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual Account Account { get; set; }
    }
}
