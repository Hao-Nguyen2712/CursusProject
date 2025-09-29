using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application
{
    public interface IRateService
    {
        // Existing methods
        List<Rate> GetAllRate();
        List<Rate> GetAllReviews(); // Alias for GetAllRate for admin reviews
        Rate GetRateById(int RateId);
        void CreateRate(Rate rate);
        void UpdateRate(Rate rate);
        void DeleteRate(int RateId);
        
        // New review management methods
        bool ApproveReview(int rateId, string moderatorId);
        bool RejectReview(int rateId, string moderatorId, string reason);
        bool BulkApproveReviews(List<int> rateIds, string moderatorId);
        bool BulkRejectReviews(List<int> rateIds, string moderatorId, string reason);
        List<Rate> GetReviewsByStatus(string status);
        List<Rate> GetReviewsByCourse(int courseId);
        List<Rate> GetPendingReviews();
        List<Rate> GetFlaggedReviews();
        List<Rate> GetRecentReviews(int days = 7);
        Dictionary<string, int> GetReviewStatistics();
        Dictionary<int, double> GetCourseAverageRatings();
        double GetCourseAverageRating(int courseId);
        List<Rate> GetReviewsByDateRange(DateTime startDate, DateTime endDate);
        List<Rate> SearchReviews(string searchTerm);
        Dictionary<int, Dictionary<int, int>> GetRatingDistribution(); // CourseId -> Rating -> Count
        List<Rate> GetTopRatedCourseReviews(int limit = 10);
        List<Rate> GetLowestRatedCourseReviews(int limit = 10);
    }
}