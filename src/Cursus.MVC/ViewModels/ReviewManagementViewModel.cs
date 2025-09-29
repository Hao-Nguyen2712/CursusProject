using Cursus.Domain.Models;

namespace Cursus.MVC.ViewModels
{
    public class ReviewManagementViewModel
    {
        // Course Reviews Section
        public List<CourseReviewDetail> CourseReviews { get; set; } = new List<CourseReviewDetail>();
        public List<CourseReviewSummary> TopRatedCourses { get; set; } = new List<CourseReviewSummary>();
        public List<CourseReviewSummary> LowestRatedCourses { get; set; } = new List<CourseReviewSummary>();
        
        // Review Statistics
        public int TotalReviews { get; set; }
        public int PendingReviews { get; set; }
        public int ApprovedReviews { get; set; }
        public int RejectedReviews { get; set; }
        public double AverageRating { get; set; }
        
        // Recent Activity
        public List<CourseReviewDetail> RecentReviews { get; set; } = new List<CourseReviewDetail>();
        public List<ReviewModerationAction> RecentModerationActions { get; set; } = new List<ReviewModerationAction>();
        
        // Filter Options
        public string StatusFilter { get; set; } = "All";
        public string RatingFilter { get; set; } = "All";
        public string DateFilter { get; set; } = "All";
        public string CourseFilter { get; set; } = "All";
        public string SearchQuery { get; set; } = "";
        
        // Pagination
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalPages { get; set; }
        
        // Calculated Properties
        public double ReviewApprovalRate => TotalReviews > 0 ? (double)ApprovedReviews / TotalReviews * 100 : 0;
        public int FiveStarReviews { get; set; }
        public int OneStarReviews { get; set; }
    }

    public class CourseReviewDetail
    {
        public int RateId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseAvatar { get; set; } = string.Empty;
        public int AccountId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public string ReviewerEmail { get; set; } = string.Empty;
        public string ReviewerAvatar { get; set; } = string.Empty;
        public int RatePoint { get; set; }
        public string RateContent { get; set; } = string.Empty;
        public DateTime RateDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
        public bool IsReported { get; set; }
        public int ReportCount { get; set; }
        public string InstructorName { get; set; } = string.Empty;
        
        // Helper Properties
        public string FormattedDate => RateDate.ToString("MMM dd, yyyy");
        public string RatingStars => new string('★', RatePoint) + new string('☆', 5 - RatePoint);
        public string StatusBadgeClass => Status switch
        {
            "Approved" => "badge-success",
            "Rejected" => "badge-danger",
            "Pending" => "badge-warning",
            _ => "badge-secondary"
        };
        public bool HasContent => !string.IsNullOrWhiteSpace(RateContent);
    }

    public class CourseReviewSummary
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseAvatar { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public int ApprovedReviews { get; set; }
        public int PendingReviews { get; set; }
        public DateTime LastReviewDate { get; set; }
        
        // Rating Distribution
        public int FiveStarCount { get; set; }
        public int FourStarCount { get; set; }
        public int ThreeStarCount { get; set; }
        public int TwoStarCount { get; set; }
        public int OneStarCount { get; set; }
        
        // Helper Properties
        public string FormattedRating => AverageRating.ToString("F1");
        public string RatingStars => GenerateStarDisplay(AverageRating);
        public string LastReviewFormatted => LastReviewDate.ToString("MMM dd, yyyy");
        
        private string GenerateStarDisplay(double rating)
        {
            var fullStars = (int)Math.Floor(rating);
            var hasHalfStar = rating - fullStars >= 0.5;
            var emptyStars = 5 - fullStars - (hasHalfStar ? 1 : 0);
            
            return new string('★', fullStars) + 
                   (hasHalfStar ? "☆" : "") + 
                   new string('☆', emptyStars);
        }
    }

    public class ReviewModerationAction
    {
        public int ActionId { get; set; }
        public int ReviewId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string ReviewerName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // Approved, Rejected, Flagged
        public string ModeratorName { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        
        // Helper Properties
        public string FormattedDate => ActionDate.ToString("MMM dd, yyyy HH:mm");
        public string ActionBadgeClass => Action switch
        {
            "Approved" => "badge-success",
            "Rejected" => "badge-danger",
            "Flagged" => "badge-warning",
            _ => "badge-secondary"
        };
    }



    public class BulkModerationRequest
    {
        public List<int> SelectedIds { get; set; } = new List<int>();
        public string Action { get; set; } = string.Empty; // approve, reject, delete
        public string Reason { get; set; } = string.Empty;
        public string ModerationType { get; set; } = string.Empty; // comment, review
    }
}