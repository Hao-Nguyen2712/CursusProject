using Cursus.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Application.Earning
{
	public interface IEarningRepository
	{
		public List<string> GetTop10Courses(string userId);
		public List<string> GetTop10EnrollCounts(string userId);
		public List<EarningAnalysisViewModel> GetTopSellingCourses(string userID);
		public double GetTotalRevenueForCurrentMonth(string userID);
		double TotalIncomebyDay(string userID);
    }
}
