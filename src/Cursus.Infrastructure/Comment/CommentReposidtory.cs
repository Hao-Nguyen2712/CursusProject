using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public Commnent addComment(Cursus.Domain.Models.Commnent comment)
		{
			_db.Commnents.Add(comment);
			_db.SaveChanges();
			return comment;
		}

		public List<Commnent> GetCommnentsByLessonID(int lessonID)
		{
			return _db.Commnents.Where(c => c.LessionId == lessonID).ToList();
		}
	}
}
