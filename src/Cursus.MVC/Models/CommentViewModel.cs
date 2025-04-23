using Cursus.Domain.Models;

namespace Cursus.MVC.Models
{
    public class CommentViewModel
    {
        public int CmtId { get; set; }
        public int? AccountId { get; set; }
        public int? LessionId { get; set; }
        public string CmtContent { get; set; }
        public int? CmtLevel { get; set; }
        public string CmtReply { get; set; }
        public DateTime? CmtDate { get; set; }

        public virtual AccountViewModel AccountVM { get; set; }
        public virtual LessonViewModel LessionVM { get; set; }
    }
}
