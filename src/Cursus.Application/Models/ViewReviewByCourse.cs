using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cursus.Application.Models.ViewReviewViewModel;

namespace Cursus.Application.Models
{
    public class ViewReviewByCourse
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int? CreatedByAccID { get; set; }
        public List<ViewReviewDetail> Reviews { get; set; }
        public double AverageRatePoint { get; set; }
        public int TotalReviews { get; set; }
        public double Percent5Star { get; set; }
        public double Percent4Star { get; set; }
        public double Percent3Star { get; set; }
        public double Percent2Star { get; set; }
        public double Percent1Star { get; set; }
    }
    public class ViewReviewDetail
    {
        public string CommenterName { get; set; }
        public string Content { get; set; }
        public int? RatePoint { get; set; }
        public string Avatar { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
