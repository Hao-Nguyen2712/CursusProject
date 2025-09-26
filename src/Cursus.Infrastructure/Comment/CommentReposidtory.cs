using Cursus.Application.Comment;
using Cursus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Cursus.Infrastructure.Comment
{
    public class CommentReposidtory : ICommentRepository
    {
        private readonly CursusDBContext _db;

        public CommentReposidtory(CursusDBContext db)
        {
            _db = db;
        }

        public Cursus.Domain.Models.Comment addComment(Cursus.Domain.Models.Comment comment)
        {
            _db.Comments.Add(comment);
            _db.SaveChanges();
            return comment;
        }
        public List<Domain.Models.Comment> GetCommentsByLessonID(int lessonID)
        {
            return _db.Comments.Where(c => c.LessionId == lessonID).ToList();
        }

        public Cursus.Domain.Models.Comment GetCommentById(int commentId)
        {
            return _db.Comments
                .Include(c => c.Account)
                .Include(c => c.Lession)
                .FirstOrDefault(c => c.CmtId == commentId);
        }

        public void UpdateComment(Cursus.Domain.Models.Comment comment)
        {
            _db.Comments.Update(comment);
            _db.SaveChanges();
        }

        public List<Cursus.Domain.Models.Comment> GetPendingComments()
        {
            return _db.Comments
                .Include(c => c.Account)
                .Include(c => c.Lession)
                .Where(c => string.IsNullOrEmpty(c.CmtContent) == false)
                .OrderByDescending(c => c.CmtDate)
                .Take(20)
                .ToList();
        }

        public List<Cursus.Domain.Models.Comment> GetAllComments()
        {
            return _db.Comments
                .Include(c => c.Account)
                .Include(c => c.Lession)
                .ToList();
        }
    }
}
