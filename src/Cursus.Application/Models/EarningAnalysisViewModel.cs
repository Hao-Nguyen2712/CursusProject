using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cursus.Application.Models.ViewReviewViewModel;

namespace Cursus.Application.Models
{
	public class EarningAnalysisViewModel
	{
		public int? CourseId { get; set; }
		public string CourseName { get; set; }
		public int EnrollCount { get; set; }
		public string CourseTitle { get; set; }
		public DateTime? EnrollDate { get; set; }
		public decimal? CoursePrice { get; set; }
		public int NumberOfSales { get; set; }
		public decimal? TotalSalesAmount { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public List<EarningAnalysis> Earnings { get; set; }

		public class EarningAnalysis
		{
			public int AccMoney{ get; set; }
			public double TotalIncomebyMonth{ get; set; }
			public double TotalIncomebyDay { get; set; }
			public string GetTop10Courses { get; set; }
			public string GetTop10EnrollCounts { get; set; }

		}
		}

}
