using Cursus.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Application.Review
{
	public interface IReviewRepository
	{
		public List<ViewReviewViewModel> GetAllReview(string UserID, string searchTerm);
        public List<ViewReviewByCourse> GetReviewByCourseId(int courseId);
    }
}
