	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Application.Models
{
	public class ViewReviewViewModel
	{
		public int CourseId { get; set; }
		public string CourseTitle { get; set; }
		public int? CreatedByAccID { get; set; }
		public List<ViewReviewDetails> Reviews { get; set; }

			
		public double AverageRatePoint { get; set; }
		public int TotalReviews { get; set; }
		public decimal Percent5Star { get; set; }
		public decimal Percent4Star { get; set; }
		public decimal Percent3Star { get; set; }
		public decimal Percent2Star { get; set; }
		public decimal Percent1Star { get; set; }
		public class ViewReviewDetails
		{
			public string CommenterName { get; set; }
			public string CourseName { get; set; }
			public string Content { get; set; }
			public int? RatePoint { get; set; }
			public string Avatar { get; set; }
			public DateTime? CreatedDate { get; set; }

		}
	}
}
