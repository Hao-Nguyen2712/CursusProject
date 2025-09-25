using Cursus.Application.Comment;
using Cursus.Domain.Models;

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
    }
}
