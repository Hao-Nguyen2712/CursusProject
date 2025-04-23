using Cursus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Application.Comment
{
	public class CommentService : ICommentService
	{
		private readonly ICommentRepository _commentRepository;

		public CommentService(ICommentRepository commentRepository)
		{
			_commentRepository = commentRepository;
		}
		public Commnent addComment(Commnent comment)
		{
			return _commentRepository.addComment(comment);
		}

		public List<Commnent> GetCommnentsByLessonID(int lessonID)
		{
			List<Commnent> comments = _commentRepository.GetCommnentsByLessonID(lessonID);
			return comments;	
		}
	}
}
