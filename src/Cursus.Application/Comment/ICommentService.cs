namespace Cursus.Application.Comment
{
    public interface ICommentService
    {

        public Cursus.Domain.Models.Comment addComment(Cursus.Domain.Models.Comment comment);
        public List<Cursus.Domain.Models.Comment> GetCommentsByLessonID(int lessonID);
    }
}
