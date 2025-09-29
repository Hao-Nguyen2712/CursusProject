using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cursus.Application.Account;
using Cursus.Application.Enroll;
using Cursus.Application.Report;
using Cursus.Application.Subscrise;
using Cursus.Domain.Models;
using Cursus.Domain.ViewModels;

namespace Cursus.Application.AdminDashBoard
{
    public class AdminDashBoardService : IAdminDashBoardService
    {
        private readonly IEnrollRepository _enrollRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAdminDashBoardRepository _adminDashBoardRepository;
        
        public AdminDashBoardService(
            IEnrollRepository enrollRepository,
            IAccountRepository accountRepository, 
            ICourseRepository courseRepository,
            IReportRepository reportRepository,
            IAdminDashBoardRepository adminDashBoardRepository)
        {
            _enrollRepository = enrollRepository;
            _courseRepository = courseRepository;
            _reportRepository = reportRepository;
            _accountRepository = accountRepository;
            _adminDashBoardRepository = adminDashBoardRepository;
        }
        public Domain.Models.AdminDashBoard GetData()
        {
            // Get all enrolls and courses for the given account ID
            var enrolls = _enrollRepository.GetAllEnroll();
            var courses = _courseRepository.GetAllCourse();
            var accounts = _accountRepository.GetAllAccount();
            var reports = _reportRepository.GetAllReport();

            // Calculate Admindashboard data
            var Admindashboard = new Domain.Models.AdminDashBoard()
            {
                TotalUsers = accounts.Count(),
                TotalRevenue = (decimal)enrolls.Sum(e => e.Course.CourseMoney),
                TotalCourses = courses.Count(),
                PendingReports = reports.Count(),
                PendingCourseList = courses.Where(c => c.CourseStatus == "Pending Approval").ToList(),
                IntructorsList = accounts.Where(a => a.Description == "Instructor").ToList()
            };

            return Admindashboard;
        }

        public RevenueAnalyticsViewModel GetRevenueAnalytics(DateTime? startDate = null, DateTime? endDate = null)
        {
            var endDateValue = endDate ?? DateTime.Now;
            var startDateValue = startDate ?? endDateValue.AddMonths(-12);

            var totalRevenue = _adminDashBoardRepository.GetTotalRevenue(startDateValue, endDateValue);
            var monthlyRevenue = _adminDashBoardRepository.GetMonthlyRevenue(endDateValue);
            var previousMonthRevenue = _adminDashBoardRepository.GetMonthlyRevenue(endDateValue.AddMonths(-1));
            
            var monthlyGrowth = previousMonthRevenue > 0 
                ? ((double)(monthlyRevenue - previousMonthRevenue) / (double)previousMonthRevenue) * 100 
                : 0;

            return new RevenueAnalyticsViewModel
            {
                TotalRevenue = totalRevenue,
                MonthlyRevenue = monthlyRevenue,
                DailyRevenue = _adminDashBoardRepository.GetDailyRevenueData(endDateValue.Date, endDateValue.Date.AddDays(1))
                    .FirstOrDefault()?.Revenue ?? 0,
                PreviousMonthRevenue = previousMonthRevenue,
                MonthlyGrowthPercentage = monthlyGrowth,
                PlatformFeePercentage = 20, // Assuming 20% platform fee
                PlatformFees = totalRevenue * 0.2m,
                InstructorEarnings = totalRevenue * 0.8m,
                TopSellingCourses = _adminDashBoardRepository.GetTopSellingCourses(10, startDateValue, endDateValue),
                PaymentMethodBreakdown = _adminDashBoardRepository.GetPaymentMethodBreakdown(startDateValue, endDateValue),
                MonthlyRevenueData = _adminDashBoardRepository.GetMonthlyRevenueData(12),
                DailyRevenueData = _adminDashBoardRepository.GetDailyRevenueData(endDateValue.AddDays(-30), endDateValue),
                TopEarningInstructors = _adminDashBoardRepository.GetTopEarningInstructors(10, startDateValue, endDateValue),
                TotalActiveInstructors = _accountRepository.GetAllAccount().Count(a => a.Description == "Instructor"),
                StartDate = startDateValue,
                EndDate = endDateValue
            };
        }

        public InstructorPayoutsViewModel GetInstructorPayouts(DateTime? startDate = null, DateTime? endDate = null)
        {
            var endDateValue = endDate ?? DateTime.Now;
            var startDateValue = startDate ?? endDateValue.AddMonths(-3);

            var pendingPayouts = _adminDashBoardRepository.GetPendingPayouts();
            var payoutHistory = _adminDashBoardRepository.GetPayoutHistory(startDateValue, endDateValue);
            var instructorEarnings = _adminDashBoardRepository.GetInstructorEarningsSummary();

            return new InstructorPayoutsViewModel
            {
                TotalPendingPayouts = pendingPayouts.Sum(p => p.Amount),
                TotalPaidOut = payoutHistory.Where(h => h.Status == PayoutStatus.Completed).Sum(h => h.Amount),
                CurrentMonthPayouts = payoutHistory
                    .Where(h => h.CompletedDate?.Month == DateTime.Now.Month && h.CompletedDate?.Year == DateTime.Now.Year)
                    .Sum(h => h.Amount),
                PendingPayoutCount = pendingPayouts.Count,
                CompletedPayoutCount = payoutHistory.Count(h => h.Status == PayoutStatus.Completed),
                PendingPayouts = pendingPayouts,
                PayoutHistory = payoutHistory,
                InstructorEarnings = instructorEarnings,
                AveragePayoutAmount = payoutHistory.Any() ? payoutHistory.Average(h => h.Amount) : 0,
                AverageProcessingTime = payoutHistory.Any(h => h.ProcessingTime.TotalDays > 0) 
                    ? TimeSpan.FromDays(payoutHistory.Where(h => h.ProcessingTime.TotalDays > 0).Average(h => h.ProcessingTime.TotalDays))
                    : TimeSpan.Zero,
                TotalActiveInstructors = instructorEarnings.Count,
                StartDate = startDateValue,
                EndDate = endDateValue
            };
        }

        public async Task<bool> ProcessPayoutAsync(int payoutId, string adminId)
        {
            var payout = _adminDashBoardRepository.GetPayoutById(payoutId);
            if (payout == null || payout.Status != PayoutStatus.Pending)
                return false;

            return await _adminDashBoardRepository.UpdatePayoutStatusAsync(payoutId, PayoutStatus.Completed, adminId);
        }

        public async Task<bool> BulkProcessPayoutsAsync(List<int> payoutIds, string adminId)
        {
            var results = new List<bool>();
            foreach (var payoutId in payoutIds)
            {
                results.Add(await ProcessPayoutAsync(payoutId, adminId));
            }
            return results.All(r => r);
        }

        public async Task<PayoutRequest> CreatePayoutRequestAsync(int instructorId, decimal amount, string paymentMethod)
        {
            return await _adminDashBoardRepository.CreatePayoutRequestAsync(instructorId, amount, paymentMethod);
        }
    }
}