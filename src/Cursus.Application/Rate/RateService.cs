using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application
{
    public class RateService : IRateService
    {
        private readonly IRateRepository _rateRepository;

        public RateService(IRateRepository rateRepository)
        {
            _rateRepository = rateRepository;
        }

        // Existing methods
        public void CreateRate(Rate rate)
        {
            _rateRepository.CreateRate(rate);
        }

        public void DeleteRate(int RateId)
        {
            _rateRepository.DeleteRate(RateId);
        }

        public List<Rate> GetAllRate()
        {
            return _rateRepository.GetAllRate();
        }

        public List<Rate> GetAllReviews()
        {
            return _rateRepository.GetAllRate();
        }

        public Rate GetRateById(int RateId)
        {
            return _rateRepository.GetRateById(RateId);
        }

        public void UpdateRate(Rate rate)
        {
            _rateRepository.UpdateRate(rate);
        }

        // New review management methods
        public bool ApproveReview(int rateId, string moderatorId)
        {
            try
            {
                var rate = _rateRepository.GetRateById(rateId);
                if (rate != null)
                {
                    // Since Rate model doesn't have status field, we'll use RateContent to store status
                    // In a real implementation, you should add a RateStatus field to the Rate model
                    rate.RateContent = rate.RateContent + " [APPROVED]";
                    _rateRepository.UpdateRate(rate);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool RejectReview(int rateId, string moderatorId, string reason)
        {
            try
            {
                var rate = _rateRepository.GetRateById(rateId);
                if (rate != null)
                {
                    rate.RateContent = rate.RateContent + " [REJECTED: " + reason + "]";
                    _rateRepository.UpdateRate(rate);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool BulkApproveReviews(List<int> rateIds, string moderatorId)
        {
            try
            {
                foreach (var id in rateIds)
                {
                    ApproveReview(id, moderatorId);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool BulkRejectReviews(List<int> rateIds, string moderatorId, string reason)
        {
            try
            {
                foreach (var id in rateIds)
                {
                    RejectReview(id, moderatorId, reason);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Rate> GetReviewsByStatus(string status)
        {
            var allRates = _rateRepository.GetAllRate();
            return status.ToLower() switch
            {
                "approved" => allRates.Where(r => r.RateContent?.Contains("[APPROVED]") == true).ToList(),
                "rejected" => allRates.Where(r => r.RateContent?.Contains("[REJECTED") == true).ToList(),
                "pending" => allRates.Where(r => !r.RateContent?.Contains("[APPROVED]") == true && 
                                                !r.RateContent?.Contains("[REJECTED") == true).ToList(),
                _ => allRates
            };
        }

        public List<Rate> GetReviewsByCourse(int courseId)
        {
            return _rateRepository.GetAllRate()
                .Where(r => r.CourseId == courseId)
                .OrderByDescending(r => r.RateDate)
                .ToList();
        }

        public List<Rate> GetPendingReviews()
        {
            return GetReviewsByStatus("pending");
        }

        public List<Rate> GetFlaggedReviews()
        {
            // In a real implementation, you'd have a proper flagging system
            // For now, we'll consider reviews with rating 1 or 2 as potentially flagged
            return _rateRepository.GetAllRate()
                .Where(r => r.RatePoint <= 2)
                .OrderByDescending(r => r.RateDate)
                .ToList();
        }

        public List<Rate> GetRecentReviews(int days = 7)
        {
            var cutoffDate = DateTime.Now.AddDays(-days);
            return _rateRepository.GetAllRate()
                .Where(r => r.RateDate >= cutoffDate)
                .OrderByDescending(r => r.RateDate)
                .ToList();
        }

        public Dictionary<string, int> GetReviewStatistics()
        {
            var allRates = _rateRepository.GetAllRate();
            var stats = new Dictionary<string, int>
            {
                {"Total", allRates.Count},
                {"Approved", allRates.Count(r => r.RateContent?.Contains("[APPROVED]") == true)},
                {"Rejected", allRates.Count(r => r.RateContent?.Contains("[REJECTED") == true)},
                {"Pending", allRates.Count(r => !r.RateContent?.Contains("[APPROVED]") == true && 
                                                !r.RateContent?.Contains("[REJECTED") == true)},
                {"Today", allRates.Count(r => r.RateDate?.Date == DateTime.Today)},
                {"ThisWeek", allRates.Count(r => r.RateDate >= DateTime.Today.AddDays(-7))},
                {"FiveStars", allRates.Count(r => r.RatePoint == 5)},
                {"OneStar", allRates.Count(r => r.RatePoint == 1)}
            };
            return stats;
        }

        public Dictionary<int, double> GetCourseAverageRatings()
        {
            return _rateRepository.GetAllRate()
                .Where(r => r.CourseId.HasValue && r.RatePoint.HasValue)
                .GroupBy(r => r.CourseId.Value)
                .ToDictionary(g => g.Key, g => g.Average(r => r.RatePoint.Value));
        }

        public List<Rate> GetReviewsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _rateRepository.GetAllRate()
                .Where(r => r.RateDate >= startDate && r.RateDate <= endDate)
                .OrderByDescending(r => r.RateDate)
                .ToList();
        }

        public List<Rate> SearchReviews(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Rate>();

            return _rateRepository.GetAllRate()
                .Where(r => r.RateContent?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true ||
                           r.Account?.FullName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true ||
                           r.Course?.CourseName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true)
                .OrderByDescending(r => r.RateDate)
                .ToList();
        }

        public Dictionary<int, Dictionary<int, int>> GetRatingDistribution()
        {
            var allRates = _rateRepository.GetAllRate()
                .Where(r => r.CourseId.HasValue && r.RatePoint.HasValue);

            var distribution = new Dictionary<int, Dictionary<int, int>>();

            foreach (var courseGroup in allRates.GroupBy(r => r.CourseId.Value))
            {
                var courseId = courseGroup.Key;
                var ratingCounts = new Dictionary<int, int>
                {
                    {1, 0}, {2, 0}, {3, 0}, {4, 0}, {5, 0}
                };

                foreach (var ratingGroup in courseGroup.GroupBy(r => r.RatePoint.Value))
                {
                    ratingCounts[ratingGroup.Key] = ratingGroup.Count();
                }

                distribution[courseId] = ratingCounts;
            }

            return distribution;
        }

        public List<Rate> GetTopRatedCourseReviews(int limit = 10)
        {
            var courseAverages = GetCourseAverageRatings()
                .OrderByDescending(kv => kv.Value)
                .Take(limit)
                .Select(kv => kv.Key);

            var topRatedReviews = new List<Rate>();
            foreach (var courseId in courseAverages)
            {
                var bestReview = _rateRepository.GetAllRate()
                    .Where(r => r.CourseId == courseId)
                    .OrderByDescending(r => r.RatePoint)
                    .ThenByDescending(r => r.RateDate)
                    .FirstOrDefault();
                
                if (bestReview != null)
                    topRatedReviews.Add(bestReview);
            }

            return topRatedReviews;
        }

        public List<Rate> GetLowestRatedCourseReviews(int limit = 10)
        {
            var courseAverages = GetCourseAverageRatings()
                .OrderBy(kv => kv.Value)
                .Take(limit)
                .Select(kv => kv.Key);

            var lowestRatedReviews = new List<Rate>();
            foreach (var courseId in courseAverages)
            {
                var worstReview = _rateRepository.GetAllRate()
                    .Where(r => r.CourseId == courseId)
                    .OrderBy(r => r.RatePoint)
                    .ThenByDescending(r => r.RateDate)
                    .FirstOrDefault();
                
                if (worstReview != null)
                    lowestRatedReviews.Add(worstReview);
            }

            return lowestRatedReviews;
        }



        public double GetCourseAverageRating(int courseId)
        {
            var courseRates = _rateRepository.GetAllRate()
                .Where(r => r.CourseId == courseId && r.RatePoint.HasValue);
            return courseRates.Any() ? courseRates.Average(r => r.RatePoint.Value) : 0.0;
        }
    }
}