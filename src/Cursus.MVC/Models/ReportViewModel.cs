namespace Cursus.MVC.Models
{
    public class ReportViewModel
    {
        public int ReportId { get; set; }
        public int? CourseId { get; set; }
        public int? CmtId { get; set; }
        public int? AccountId { get; set; }
        public string ReportReasson { get; set; }
        public DateTime? ReportDate { get; set; }
    }
}
