using Cursus.Domain.Models;

namespace Cursus.MVC.ViewModels
{
    public class CommentModerationViewModel
    {
        public List<CommentDetail> Comments { get; set; } = new List<CommentDetail>();
        public int TotalComments { get; set; }
        public int PendingComments { get; set; }
        public int ApprovedComments { get; set; }
        public int RejectedComments { get; set; }
        
        // Pagination
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalPages { get; set; }
        
        // Filtering
        public string StatusFilter { get; set; } = "";
        public string SearchQuery { get; set; } = "";
        
        // Statistics
        public double ApprovalRate => TotalComments > 0 ? (double)ApprovedComments / TotalComments * 100 : 0;
        public int TodayComments { get; set; }
        public int WeekComments { get; set; }
        public int FlaggedComments { get; set; }
    }
    
    public class CommentDetail
    {
        public int CmtId { get; set; }
        public int AccountId { get; set; }
        public string CommenterName { get; set; } = "";
        public string CommenterEmail { get; set; } = "";
        public int LessionId { get; set; }
        public string LessonTitle { get; set; } = "";
        public string CourseName { get; set; } = "";
        public string CmtContent { get; set; } = "";
        public DateTime CmtDate { get; set; }
        public string Status { get; set; } = "";
        public int ReportCount { get; set; }
    }
}