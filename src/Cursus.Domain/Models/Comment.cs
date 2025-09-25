using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Comment
    {
        public Comment()
        {
            Reports = new HashSet<Report>();
        }

        [Key]
        public int CmtId { get; set; }
        public int? AccountId { get; set; }
        public int? LessionId { get; set; }
        public string CmtContent { get; set; }
        public int? CmtLevel { get; set; }
        public string CmtReply { get; set; }
        public DateTime? CmtDate { get; set; }

        public virtual Account Account { get; set; }
        public virtual Lesson Lession { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
