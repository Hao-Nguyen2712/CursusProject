using Cursus.Application.Credits;
using Cursus.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Application.Earning
{
    public class EarningService : IEarningService
    {
        private readonly IEarningRepository _repository;

        public EarningService(IEarningRepository repository)
        {
            _repository = repository;
        }

		public List<EarningAnalysisViewModel> GetTopSellingCourses(string userID)
		{ 
            return _repository.GetTopSellingCourses(userID).ToList();
		}
		public double GetTotalRevenueForCurrentMonth(string userID)
        {
            double totalMonth = _repository.GetTotalRevenueForCurrentMonth(userID);
            return totalMonth;
        }


		public double TotalIncomebyDay(string userID)
        {
            double sum = _repository.TotalIncomebyDay( userID);
            return sum;
        }

		public List<string> GetTop10Courses(string userId)
		{
			var listTopCourse = _repository.GetTop10Courses(userId).ToList();
			return listTopCourse;
		}

		public List<string> GetTop10EnrollCounts(string userId)
		{
			var listTopCourseEnroll = _repository.GetTop10EnrollCounts(userId).ToList();
			return listTopCourseEnroll;
		}
	}
}
