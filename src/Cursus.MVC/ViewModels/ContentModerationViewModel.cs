using Cursus.Domain.Models;

namespace Cursus.MVC.ViewModels
{
    public class ContentModerationViewModel
    {
        // Comments Moderation
        public List<Comment> PendingComments { get; set; } = new List<Comment>();
        public int TotalComments { get; set; }
        public int PendingCommentsCount { get; set; }
        public int ApprovedCommentsCount { get; set; }
        public int RejectedCommentsCount { get; set; }
        
        // Reports Moderation
        public List<Report> ReportedContent { get; set; } = new List<Report>();
        public int TotalReports { get; set; }
        public int PendingReportsCount { get; set; }
        public int ResolvedReportsCount { get; set; }
        
        // Calculated Properties
        public double CommentApprovalRate => TotalComments > 0 ? (double)ApprovedCommentsCount / TotalComments * 100 : 0;
        public double ReportResolutionRate => TotalReports > 0 ? (double)ResolvedReportsCount / TotalReports * 100 : 0;
        
        // Convenience Properties for View
        public double ApprovalRate => CommentApprovalRate;
        public int ReportedContentCount => ReportedContent.Count;
        public int ApprovedTodayCount { get; set; }
    }
}