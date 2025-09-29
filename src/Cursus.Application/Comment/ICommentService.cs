namespace Cursus.Application.Comment
{
    public interface ICommentService
    {
        public Cursus.Domain.Models.Comment addComment(Cursus.Domain.Models.Comment comment);
        public List<Cursus.Domain.Models.Comment> GetCommentsByLessonID(int lessonID);
        public Cursus.Domain.Models.Comment GetCommentById(int commentId);
        public void UpdateComment(Cursus.Domain.Models.Comment comment);
        public List<Cursus.Domain.Models.Comment> GetPendingComments();
        public List<Cursus.Domain.Models.Comment> GetAllComments();
        
        // New moderation methods
        public bool ApproveComment(int commentId, string moderatorId);
        public bool RejectComment(int commentId, string moderatorId, string reason);
        public bool BulkApproveComments(List<int> commentIds, string moderatorId);
        public bool BulkRejectComments(List<int> commentIds, string moderatorId, string reason);
        public bool DeleteComment(int commentId);
        public List<Cursus.Domain.Models.Comment> GetCommentsByStatus(string status);
        public List<Cursus.Domain.Models.Comment> GetFlaggedComments();
        public List<Cursus.Domain.Models.Comment> GetRecentComments(int days = 7);
        public Dictionary<string, int> GetCommentStatistics();
        public List<Cursus.Domain.Models.Comment> GetCommentsByDateRange(DateTime startDate, DateTime endDate);
        public List<Cursus.Domain.Models.Comment> SearchComments(string searchTerm);
    }
}
