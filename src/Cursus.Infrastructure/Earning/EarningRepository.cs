using Cursus.Application.Earning;
using Cursus.Application.Models;
using Cursus.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Infrastructure.Earning
{
	public class EarningRepository : IEarningRepository
	{
		private readonly CursusDBContext _dbContext;

		public EarningRepository(CursusDBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public List<string> GetTop10Courses(string userId)
		{
			// Lấy accountId từ userId
			Domain.Models.Account account = _dbContext.Accounts.FirstOrDefault(x => x.Id == userId);

			if (account == null)
			{
				throw new Exception("Không tìm thấy thông tin tài khoản.");
			}

			// Thực hiện truy vấn để lấy top 10 khóa học được enroll nhiều nhất
			var topCourseNames = _dbContext.Enrolls
				.Where(e => e.Course.AccountId == account.AccountId) // Lọc theo AccountId của người dùng
				.GroupBy(e => e.CourseId)
				.Select(g => new
				{
					CourseId = g.Key,
					EnrollCount = g.Count()
				})
				.OrderByDescending(g => g.EnrollCount)
				.Take(10)
				.Join(_dbContext.Courses,
					  g => g.CourseId,
					  c => c.CourseId,
					  (g, c) => c.CourseName)
				.ToList();

			return topCourseNames;
		}

		public List<string> GetTop10EnrollCounts(string userId)
		{
			// Lấy accountId từ userId
			Domain.Models.Account account = _dbContext.Accounts.FirstOrDefault(x => x.Id == userId);

			if (account == null)
			{
				throw new Exception("Không tìm thấy thông tin tài khoản.");
			}

			// Truy vấn để lấy số lần enroll của top 10 khóa học được enroll nhiều nhất
			var topEnrollCounts = _dbContext.Enrolls
				.Where(e => e.Course.AccountId == account.AccountId) // Lọc theo AccountId của người dùng
				.GroupBy(e => e.CourseId)
				.Select(g => new
				{
					EnrollCount = g.Count()
				})
				.OrderByDescending(g => g.EnrollCount)
				.Take(10)
				.Select(g => g.EnrollCount.ToString()) // Chuyển đổi số lần enroll thành chuỗi
				.ToList();

			return topEnrollCounts;
		}


		public List<EarningAnalysisViewModel> GetTopSellingCourses(string userID)
		{
			Domain.Models.Account account = _dbContext.Accounts.FirstOrDefault(x => x.Id == userID);
            if (account == null)
            {
                throw new Exception("Không tìm thấy thông tin tài khoản.");
            }
            var soldCourses = _dbContext.Enrolls
				.Where(e => e.CourseId != null && e.Course.AccountId == account.AccountId) // Đảm bảo có người đăng ký vào khóa học và khóa học do người dùng tạo ra
				.GroupBy(e => e.CourseId)
				.Select(g => new EarningAnalysisViewModel
				{
					CourseTitle = g.FirstOrDefault().Course.CourseName,
					EnrollDate = g.FirstOrDefault().EnrollDate,
					CoursePrice = g.FirstOrDefault().Course.CourseMoney,
					NumberOfSales = g.Count(),
					TotalSalesAmount = g.Count() * g.FirstOrDefault().Course.CourseMoney
				})
				.OrderByDescending(c => c.TotalSalesAmount)
				.ToList();

			return soldCourses;
		}


		public double GetTotalRevenueForCurrentMonth(string userID)
		{
			Domain.Models.Account account = _dbContext.Accounts.FirstOrDefault(x => x.Id == userID);
            if (account == null)
            {
                throw new Exception("Không tìm thấy thông tin tài khoản.");
            }
            var currentMonth = DateTime.Now.Month;
			var currentYear = DateTime.Now.Year;

			// Tính tổng thu nhập từ các khóa học do user tạo
			var courseIds = _dbContext.Courses
			.Where(c => c.AccountId == account.AccountId)
			.Select(c => c.CourseId)
			.ToList();

			// Tính tổng thu nhập từ các khóa học do người dùng tạo trong tháng hiện tại
			var totalIncome = _dbContext.Enrolls
				.Where(e => e.EnrollDate.HasValue
							&& e.EnrollDate.Value.Month == currentMonth
							&& e.EnrollDate.Value.Year == currentYear
							&& courseIds.Contains(e.CourseId.Value))
				.Join(_dbContext.Courses,
					e => e.CourseId,
					c => c.CourseId,
					(e, c) => c.CourseMoney)
				.Sum();

			return (double)(totalIncome);
		}

		public double TotalIncomebyDay(string userID)
		{
			Domain.Models.Account account = _dbContext.Accounts.FirstOrDefault(x => x.Id == userID);

			if (account == null)
			{
				throw new Exception("Không tìm thấy thông tin tài khoản.");
			}

			DateTime today = DateTime.Today; // Lấy chỉ phần ngày, không bao gồm thời gian
			int currentYear = today.Year;
			int currentMonth = today.Month;
			int currentDay = today.Day;

			var totalMoney = _dbContext.Enrolls
				.Join(_dbContext.Courses,
					  e => e.CourseId,
					  c => c.CourseId,
					  (e, c) => new { e, c })
				.Where(ec =>
					ec.e.EnrollDate.HasValue &&
					ec.e.EnrollDate.Value.Year == currentYear &&
					ec.e.EnrollDate.Value.Month == currentMonth &&
					ec.e.EnrollDate.Value.Day == currentDay &&
					ec.c.AccountId == account.AccountId) // Thêm điều kiện lọc theo AccountId
				.Sum(ec => (decimal?)ec.c.CourseMoney) ?? 0; // Xử lý giá trị nullable

			return Convert.ToDouble(totalMoney);
		}
	}
}
