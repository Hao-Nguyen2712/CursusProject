namespace Cursus.Application.Comment
{
    public interface ICommentRepository
    {
        public Cursus.Domain.Models.Comment addComment(Cursus.Domain.Models.Comment comment);
        public List<Cursus.Domain.Models.Comment> GetCommentsByLessonID(int lessonID);
        public Cursus.Domain.Models.Comment GetCommentById(int commentId);
        public void UpdateComment(Cursus.Domain.Models.Comment comment);
        public List<Cursus.Domain.Models.Comment> GetPendingComments();
        public List<Cursus.Domain.Models.Comment> GetAllComments();
    }
}
