using Cursus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Application.Comment
{
	public interface ICommentService
	{

		public Commnent addComment(Cursus.Domain.Models.Commnent comment);
		public List<Commnent> GetCommnentsByLessonID(int lessonID);
	}
}
