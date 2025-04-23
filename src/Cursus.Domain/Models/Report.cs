using System;
using System.Collections.Generic;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public int? CourseId { get; set; }
        public int? CmtId { get; set; }
        public int? AccountId { get; set; }
        public string ReportReasson { get; set; }
        public DateTime? ReportDate { get; set; }
    }
}
