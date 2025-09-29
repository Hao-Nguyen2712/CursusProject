
namespace Cursus.Application.Comment
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        
        public Cursus.Domain.Models.Comment addComment(Cursus.Domain.Models.Comment comment)
        {
            return _commentRepository.addComment(comment);
        }

        public List<Domain.Models.Comment> GetCommentsByLessonID(int lessonID)
        {
            List<Cursus.Domain.Models.Comment> comments = _commentRepository.GetCommentsByLessonID(lessonID);
            return comments;
        }

        public Cursus.Domain.Models.Comment GetCommentById(int commentId)
        {
            return _commentRepository.GetCommentById(commentId);
        }

        public void UpdateComment(Cursus.Domain.Models.Comment comment)
        {
            _commentRepository.UpdateComment(comment);
        }

        public List<Cursus.Domain.Models.Comment> GetPendingComments()
        {
            return _commentRepository.GetPendingComments();
        }

        public List<Cursus.Domain.Models.Comment> GetAllComments()
        {
            return _commentRepository.GetAllComments();
        }

        // New moderation methods
        public bool ApproveComment(int commentId, string moderatorId)
        {
            try
            {
                var comment = _commentRepository.GetCommentById(commentId);
                if (comment != null)
                {
                    // Since Comment model doesn't have status field, we'll use CmtReply to store status
                    // In a real implementation, you should add a CmtStatus field to the Comment model
                    comment.CmtReply = comment.CmtReply + " [APPROVED]";
                    _commentRepository.UpdateComment(comment);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool RejectComment(int commentId, string moderatorId, string reason)
        {
            try
            {
                var comment = _commentRepository.GetCommentById(commentId);
                if (comment != null)
                {
                    // Since Comment model doesn't have status field, we'll use CmtReply to store status
                    comment.CmtReply = comment.CmtReply + " [REJECTED: " + reason + "]";
                    _commentRepository.UpdateComment(comment);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool BulkApproveComments(List<int> commentIds, string moderatorId)
        {
            try
            {
                foreach (var id in commentIds)
                {
                    ApproveComment(id, moderatorId);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool BulkRejectComments(List<int> commentIds, string moderatorId, string reason)
        {
            try
            {
                foreach (var id in commentIds)
                {
                    RejectComment(id, moderatorId, reason);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteComment(int commentId)
        {
            try
            {
                return _commentRepository.DeleteComment(commentId);
            }
            catch
            {
                return false;
            }
        }

        public List<Cursus.Domain.Models.Comment> GetCommentsByStatus(string status)
        {
            // Since Comment model doesn't have status, we'll filter by CmtReply content
            var allComments = _commentRepository.GetAllComments();
            return status.ToLower() switch
            {
                "approved" => allComments.Where(c => c.CmtReply?.Contains("[APPROVED]") == true).ToList(),
                "rejected" => allComments.Where(c => c.CmtReply?.Contains("[REJECTED") == true).ToList(),
                "pending" => allComments.Where(c => !c.CmtReply?.Contains("[APPROVED]") == true && 
                                                   !c.CmtReply?.Contains("[REJECTED") == true).ToList(),
                _ => allComments
            };
        }

        public List<Cursus.Domain.Models.Comment> GetFlaggedComments()
        {
            // Get comments that have reports
            return _commentRepository.GetAllComments()
                .Where(c => c.Reports != null && c.Reports.Any())
                .ToList();
        }

        public List<Cursus.Domain.Models.Comment> GetRecentComments(int days = 7)
        {
            var cutoffDate = DateTime.Now.AddDays(-days);
            return _commentRepository.GetAllComments()
                .Where(c => c.CmtDate >= cutoffDate)
                .OrderByDescending(c => c.CmtDate)
                .ToList();
        }

        public Dictionary<string, int> GetCommentStatistics()
        {
            var allComments = _commentRepository.GetAllComments();
            var stats = new Dictionary<string, int>
            {
                {"Total", allComments.Count},
                {"Approved", allComments.Count(c => c.CmtReply?.Contains("[APPROVED]") == true)},
                {"Rejected", allComments.Count(c => c.CmtReply?.Contains("[REJECTED") == true)},
                {"Pending", allComments.Count(c => !c.CmtReply?.Contains("[APPROVED]") == true && 
                                                  !c.CmtReply?.Contains("[REJECTED") == true)},
                {"Today", allComments.Count(c => c.CmtDate?.Date == DateTime.Today)},
                {"ThisWeek", allComments.Count(c => c.CmtDate >= DateTime.Today.AddDays(-7))},
                {"Flagged", allComments.Count(c => c.Reports != null && c.Reports.Any())}
            };
            return stats;
        }

        public List<Cursus.Domain.Models.Comment> GetCommentsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _commentRepository.GetAllComments()
                .Where(c => c.CmtDate >= startDate && c.CmtDate <= endDate)
                .OrderByDescending(c => c.CmtDate)
                .ToList();
        }

        public List<Cursus.Domain.Models.Comment> SearchComments(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Cursus.Domain.Models.Comment>();

            return _commentRepository.GetAllComments()
                .Where(c => c.CmtContent?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true ||
                           c.Account?.FullName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true)
                .OrderByDescending(c => c.CmtDate)
                .ToList();
        }
    }
}
