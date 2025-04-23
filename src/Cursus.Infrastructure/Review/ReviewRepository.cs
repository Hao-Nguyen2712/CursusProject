using Cursus.Application.Models;
using Cursus.Application.Review;
using Cursus.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cursus.Application.Models.ViewReviewViewModel;

namespace Cursus.Infrastructure.Review
{
	public class ReviewRepository : IReviewRepository
	{
		public readonly CursusDBContext _dbContext;

		public ReviewRepository(CursusDBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public List<ViewReviewViewModel> GetAllReview(string userID, string searchTerm)
		{

			Domain.Models.Account account = _dbContext.Accounts.FirstOrDefault(x => x.Id == userID);
			if (account == null)
			{
				return null;
			}

			int accID = account.AccountId;

			var courses = _dbContext.Courses
				.Where(course => course.AccountId == accID)
				.Where(course => _dbContext.Rates.Any(rate => rate.CourseId == course.CourseId)) // Chỉ chọn các khóa học có đánh giá
				.Select(course => new ViewReviewViewModel
				{
					CourseId = course.CourseId,
					CourseTitle = course.CourseName,
					CreatedByAccID = course.AccountId,
					Reviews = _dbContext.Rates
						.Where(rate => rate.CourseId == course.CourseId)
						.Join(_dbContext.Accounts, rate => rate.AccountId, account => account.AccountId,
							(rate, account) => new ViewReviewDetails
							{
								CommenterName = account.FullName,
								CourseName = course.CourseName, // Thêm CourseName để có thể tìm kiếm
								Content = rate.RateContent,
								RatePoint = rate.RatePoint,
								Avatar = account.Avatar,
								CreatedDate = rate.RateDate
							})
						.Where(r => string.IsNullOrEmpty(searchTerm) || r.CommenterName.Contains(searchTerm) || r.CourseName.Contains(searchTerm) || r.Content.Contains(searchTerm))
						.OrderByDescending(r => r.CreatedDate)
						.ToList()
				})
				.Where(c => c.Reviews.Any()) // Chỉ chọn các khóa học có đánh giá sau khi lọc
				.ToList();

			if (courses.Count > 0)
			{
				List<ViewReviewDetails> allReviews = courses.SelectMany(c => c.Reviews).ToList();

				double averageRatePoint = allReviews.Average(r => r.RatePoint ?? 0);

				int totalReviews = courses.SelectMany(c => c.Reviews).Count();
				decimal totalPercent5Star = totalReviews > 0 ? (decimal)courses.Sum(c => c.Reviews.Count(r => r.RatePoint == 5)) / totalReviews * 100 : 0;
				decimal totalPercent4Star = totalReviews > 0 ? (decimal)courses.Sum(c => c.Reviews.Count(r => r.RatePoint == 4)) / totalReviews * 100 : 0;
				decimal totalPercent3Star = totalReviews > 0 ? (decimal)courses.Sum(c => c.Reviews.Count(r => r.RatePoint == 3)) / totalReviews * 100 : 0;
				decimal totalPercent2Star = totalReviews > 0 ? (decimal)courses.Sum(c => c.Reviews.Count(r => r.RatePoint == 2)) / totalReviews * 100 : 0;
				decimal totalPercent1Star = totalReviews > 0 ? (decimal)courses.Sum(c => c.Reviews.Count(r => r.RatePoint == 1)) / totalReviews * 100 : 0;

				courses.ForEach(course =>
				{
					course.TotalReviews = totalReviews;
					course.Percent5Star = Math.Round(totalPercent5Star, 2);
					course.Percent4Star = Math.Round(totalPercent4Star, 2);
					course.Percent3Star = Math.Round(totalPercent3Star, 2);
					course.Percent2Star = Math.Round(totalPercent2Star, 2);
					course.Percent1Star = Math.Round(totalPercent1Star, 2);
					course.AverageRatePoint = averageRatePoint;
				});
			}
			else
			{
				return new List<ViewReviewViewModel>(); // Trả về danh sách rỗng nếu không có khóa học nào
			}

			return courses;
		}
		public List<ViewReviewByCourse> GetReviewByCourseId(int courseId)
		{
			var course = _dbContext.Courses.FirstOrDefault(c => c.CourseId == courseId);
			if (course == null)
			{
				return null;
			}

			var reviews = _dbContext.Rates
				.Where(rate => rate.CourseId == courseId)
				.Join(_dbContext.Accounts, rate => rate.AccountId, account => account.AccountId,
					(rate, account) => new ViewReviewDetail
					{
						CommenterName = account.FullName,
						Content = rate.RateContent,
						RatePoint = rate.RatePoint,
						Avatar = account.Avatar,
						CreatedDate = rate.RateDate
					}).OrderByDescending(r => r.CreatedDate)
				.ToList();

			if (reviews.Count > 0)
			{
				double averageRatePoint = reviews.Average(r => r.RatePoint ?? 0);
				int totalReviews = reviews.Count;
				double percent5Star = totalReviews > 0 ? (double)reviews.Count(r => r.RatePoint == 5) / totalReviews * 100 : 0;
				double percent4Star = totalReviews > 0 ? (double)reviews.Count(r => r.RatePoint == 4) / totalReviews * 100 : 0;
				double percent3Star = totalReviews > 0 ? (double)reviews.Count(r => r.RatePoint == 3) / totalReviews * 100 : 0;
				double percent2Star = totalReviews > 0 ? (double)reviews.Count(r => r.RatePoint == 2) / totalReviews * 100 : 0;
				double percent1Star = totalReviews > 0 ? (double)reviews.Count(r => r.RatePoint == 1) / totalReviews * 100 : 0;

				return new List<ViewReviewByCourse>
		{
			new ViewReviewByCourse
			{
				CourseId = course.CourseId,
				CourseTitle = course.CourseName,
				CreatedByAccID = course.AccountId,
				Reviews = reviews,
				AverageRatePoint = Math.Round(averageRatePoint, 2),
				TotalReviews = totalReviews,
				Percent5Star = Math.Round(percent5Star, 2),
				Percent4Star = Math.Round(percent4Star, 2),
				Percent3Star = Math.Round(percent3Star, 2),
				Percent2Star = Math.Round(percent2Star, 2),
				Percent1Star = Math.Round(percent1Star, 2)
			}
		};
			}
			else
			{
				return null;
			}

			return new List<ViewReviewByCourse>();
		}


	}
}
