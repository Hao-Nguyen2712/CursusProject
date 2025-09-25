
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
    }
}
