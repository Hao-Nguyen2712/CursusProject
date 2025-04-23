using Cursus.Application.Models;
using Cursus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Application.Review
{
	public class ReviewService : IReviewService
	{
		private readonly IReviewRepository _repository;

		public ReviewService(IReviewRepository repository)
		{
			_repository = repository;
		}

		public List<ViewReviewViewModel> GetAllReview(string UserID, string searchTerm)
		{
			var review = _repository.GetAllReview(UserID, searchTerm);
			return review;
		}
        public List<ViewReviewByCourse> GetReviewByCourseId(int courseId)
        {
            return _repository.GetReviewByCourseId(courseId);
        }

    }
}
