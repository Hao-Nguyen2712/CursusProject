using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.AdminDashBoard;
using Cursus.Domain.Models;
using Cursus.Domain.ViewModels;
using Cursus.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Cursus.Infrastructure.AdminDashBoard
{
    public class AdminDashBoardRepository : IAdminDashBoardRepository
    {
        private readonly CursusDBContext _context;

        public AdminDashBoardRepository(CursusDBContext context)
        {
            _context = context;
        }

        public Domain.Models.AdminDashBoard GetData()
        {
            var enrolls = _context.Enrolls.Include(e => e.Course).ToList();
            var courses = _context.Courses.ToList();
            var accounts = _context.Accounts.ToList();
            var reports = _context.Reports.ToList();

            return new Domain.Models.AdminDashBoard()
            {
                TotalUsers = accounts.Count,
                TotalRevenue = (decimal)enrolls.Sum(e => e.Course.CourseMoney),
                TotalCourses = courses.Count,
                PendingReports = reports.Count,
                PendingCourseList = courses.Where(c => c.CourseStatus == "Pending Approval").ToList(),
                IntructorsList = accounts.Where(a => a.Description == "Instructor").ToList()
            };
        }

        public decimal GetTotalRevenue(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Tradings.AsQueryable();
            
            if (startDate.HasValue)
                query = query.Where(t => t.TdDate >= startDate.Value);
            
            if (endDate.HasValue)
                query = query.Where(t => t.TdDate <= endDate.Value);

            return query.Sum(t => t.TdMoney) ?? 0m;
        }

        public decimal GetMonthlyRevenue(DateTime? month = null)
        {
            var targetMonth = month ?? DateTime.Now;
            return _context.Tradings
                .Where(t => t.TdDate.HasValue && t.TdDate.Value.Month == targetMonth.Month && t.TdDate.Value.Year == targetMonth.Year)
                .Sum(t => t.TdMoney) ?? 0m;
        }

        public List<TopSellingCourse> GetTopSellingCourses(int count = 10, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Enrolls
                .Include(e => e.Course)
                .ThenInclude(c => c.Account)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(e => e.EnrollDate.HasValue && e.EnrollDate.Value >= startDate.Value);
            
            if (endDate.HasValue)
                query = query.Where(e => e.EnrollDate.HasValue && e.EnrollDate.Value <= endDate.Value);

            return query
                .GroupBy(e => new { e.Course.CourseId, e.Course.CourseName, e.Course.Account.FullName, e.Course.CourseMoney })
                .Select(g => new TopSellingCourse
                {
                    CourseId = g.Key.CourseId,
                    CourseName = g.Key.CourseName,
                    CourseTitle = g.Key.CourseName,
                    InstructorName = g.Key.FullName,
                    TotalSales = g.Count(),
                    CoursePrice = g.Key.CourseMoney,
                    TotalRevenue = g.Key.CourseMoney * g.Count(),
                    LastSaleDate = g.Max(e => e.EnrollDate ?? DateTime.MinValue)
                })
                .OrderByDescending(c => c.TotalSales)
                .Take(count)
                .ToList();
        }

        public List<PaymentMethodRevenue> GetPaymentMethodBreakdown(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Tradings.AsQueryable();
            
            if (startDate.HasValue)
                query = query.Where(t => t.TdDate.HasValue && t.TdDate.Value >= startDate.Value);
            
            if (endDate.HasValue)
                query = query.Where(t => t.TdDate.HasValue && t.TdDate.Value <= endDate.Value);

            var totalRevenue = query.Sum(t => t.TdMoney) ?? 0m;

            return query
                .GroupBy(t => t.TdMethodPayment)
                .Select(g => new PaymentMethodRevenue
                {
                    PaymentMethod = g.Key,
                    Revenue = g.Sum(t => t.TdMoney) ?? 0m,
                    TransactionCount = g.Count(),
                    Percentage = totalRevenue > 0 ? (double)((g.Sum(t => t.TdMoney) ?? 0m) / totalRevenue * 100) : 0
                })
                .OrderByDescending(p => p.Revenue)
                .ToList();
        }

        public List<MonthlyRevenueData> GetMonthlyRevenueData(int months = 12)
        {
            var startDate = DateTime.Now.AddMonths(-months);
            
            return _context.Tradings
                .Where(t => t.TdDate.HasValue && t.TdDate.Value >= startDate)
                .GroupBy(t => new { t.TdDate.Value.Year, t.TdDate.Value.Month })
                .Select(g => new MonthlyRevenueData
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                    Revenue = g.Sum(t => t.TdMoney) ?? 0m,
                    TransactionCount = g.Count(),
                    PlatformFees = (g.Sum(t => t.TdMoney) ?? 0m) * 0.2m,
                    InstructorEarnings = (g.Sum(t => t.TdMoney) ?? 0m) * 0.8m
                })
                .OrderBy(m => m.Year)
                .ThenBy(m => m.Month)
                .ToList();
        }

        public List<DailyRevenueData> GetDailyRevenueData(DateTime? startDate = null, DateTime? endDate = null)
        {
            var start = startDate ?? DateTime.Now.AddDays(-30);
            var end = endDate ?? DateTime.Now;

            return _context.Tradings
                .Where(t => t.TdDate.HasValue && t.TdDate.Value >= start && t.TdDate.Value <= end)
                .GroupBy(t => t.TdDate.Value.Date)
                .Select(g => new DailyRevenueData
                {
                    Date = g.Key,
                    Revenue = g.Sum(t => t.TdMoney) ?? 0m,
                    TransactionCount = g.Count(),
                    PlatformFees = (g.Sum(t => t.TdMoney) ?? 0m) * 0.2m,
                    InstructorEarnings = (g.Sum(t => t.TdMoney) ?? 0m) * 0.8m
                })
                .OrderBy(d => d.Date)
                .ToList();
        }

        public List<TopEarningInstructor> GetTopEarningInstructors(int count = 10, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Enrolls
                .Include(e => e.Course)
                .ThenInclude(c => c.Account)
                .Where(e => e.Course.Account.Description == "Instructor")
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(e => e.EnrollDate.HasValue && e.EnrollDate.Value >= startDate.Value);
            
            if (endDate.HasValue)
                query = query.Where(e => e.EnrollDate.HasValue && e.EnrollDate.Value <= endDate.Value);

            return query
                .GroupBy(e => new { e.Course.Account.AccountId, e.Course.Account.FullName, e.Course.Account.Email })
                .Select(g => new TopEarningInstructor
                {
                    InstructorId = g.Key.AccountId,
                    InstructorName = g.Key.FullName,
                    Email = g.Key.Email,
                    TotalEarnings = g.Sum(e => e.Course.CourseMoney) * 0.8m, // 80% to instructor
                    CoursesCount = g.Select(e => e.Course.CourseId).Distinct().Count(),
                    TotalStudents = g.Count(),
                    AverageRating = 4.5m, // Placeholder - would need actual rating calculation
                    LastPayoutDate = DateTime.Now.AddDays(-30) // Placeholder
                })
                .OrderByDescending(i => i.TotalEarnings)
                .Take(count)
                .ToList();
        }

        public List<PayoutRequest> GetPendingPayouts()
        {
            // Since there's no Payout table yet, return mock data based on instructor earnings
            var instructorEarnings = GetInstructorEarningsSummary();
            
            return instructorEarnings
                .Where(i => i.AvailableBalance > 100) // Only show if they have significant balance
                .Select(i => new PayoutRequest
                {
                    PayoutId = i.InstructorId * 1000, // Mock ID
                    InstructorId = i.InstructorId,
                    InstructorName = i.InstructorName,
                    InstructorEmail = i.Email,
                    Amount = i.AvailableBalance,
                    Status = PayoutStatus.Pending,
                    RequestDate = DateTime.Now.AddDays(-Random.Shared.Next(1, 30)),
                    PaymentMethod = "Bank Transfer",
                    PaymentDetails = "****1234",
                    Notes = "Monthly payout request"
                })
                .Take(10)
                .ToList();
        }

        public List<PayoutHistory> GetPayoutHistory(DateTime? startDate = null, DateTime? endDate = null)
        {
            // Mock payout history data
            var instructors = _context.Accounts.Where(a => a.Description == "Instructor").Take(10).ToList();
            var history = new List<PayoutHistory>();

            foreach (var instructor in instructors)
            {
                for (int i = 0; i < 3; i++)
                {
                    var requestDate = DateTime.Now.AddDays(-Random.Shared.Next(30, 90));
                    var completedDate = requestDate.AddDays(Random.Shared.Next(1, 7));
                    
                    history.Add(new PayoutHistory
                    {
                        PayoutId = instructor.AccountId * 100 + i,
                        InstructorId = instructor.AccountId,
                        InstructorName = instructor.FullName,
                        Amount = Random.Shared.Next(100, 1000),
                        Status = PayoutStatus.Completed,
                        RequestDate = requestDate,
                        CompletedDate = completedDate,
                        PaymentMethod = "Bank Transfer",
                        TransactionReference = $"TXN{Random.Shared.Next(100000, 999999)}",
                        ProcessedBy = "Admin",
                        ProcessingTime = completedDate - requestDate
                    });
                }
            }

            return history.OrderByDescending(h => h.CompletedDate).ToList();
        }

        public List<InstructorEarningsSummary> GetInstructorEarningsSummary()
        {
            return _context.Accounts
                .Where(a => a.Description == "Instructor")
                .Select(a => new InstructorEarningsSummary
                {
                    InstructorId = a.AccountId,
                    InstructorName = a.FullName,
                    Email = a.Email,
                    TotalEarnings = _context.Enrolls
                        .Where(e => e.Course.AccountId == a.AccountId)
                        .Sum(e => e.Course.CourseMoney) * 0.8m,
                    PaidOut = Random.Shared.Next(100, 500), // Mock data
                    PendingPayout = 0,
                    AvailableBalance = (_context.Enrolls
                        .Where(e => e.Course.AccountId == a.AccountId)
                        .Sum(e => e.Course.CourseMoney) * 0.8m) - Random.Shared.Next(100, 500),
                    CoursesCount = _context.Courses.Count(c => c.AccountId == a.AccountId),
                    TotalStudents = _context.Enrolls.Count(e => e.Course.AccountId == a.AccountId),
                    LastPayoutDate = DateTime.Now.AddDays(-Random.Shared.Next(30, 90)),
                    AverageMonthlyEarnings = (_context.Enrolls
                        .Where(e => e.Course.AccountId == a.AccountId)
                        .Sum(e => e.Course.CourseMoney) * 0.8m) / 12
                })
                .Where(i => i.TotalEarnings > 0)
                .OrderByDescending(i => i.TotalEarnings)
                .ToList();
        }

        public async Task<bool> UpdatePayoutStatusAsync(int payoutId, PayoutStatus status, string processedBy)
        {
            // Mock implementation - in real scenario would update database
            await Task.Delay(100); // Simulate async operation
            return true;
        }

        public async Task<PayoutRequest> CreatePayoutRequestAsync(int instructorId, decimal amount, string paymentMethod)
        {
            // Mock implementation - in real scenario would create database record
            await Task.Delay(100);
            
            var instructor = _context.Accounts.FirstOrDefault(a => a.AccountId == instructorId);
            if (instructor == null) return null;

            return new PayoutRequest
            {
                PayoutId = Random.Shared.Next(10000, 99999),
                InstructorId = instructorId,
                InstructorName = instructor.FullName,
                InstructorEmail = instructor.Email,
                Amount = amount,
                Status = PayoutStatus.Pending,
                RequestDate = DateTime.Now,
                PaymentMethod = paymentMethod,
                PaymentDetails = "****1234",
                Notes = "Created via admin panel"
            };
        }

        public PayoutRequest GetPayoutById(int payoutId)
        {
            // Mock implementation - return pending payout if exists
            var pendingPayouts = GetPendingPayouts();
            return pendingPayouts.FirstOrDefault(p => p.PayoutId == payoutId);
        }
    }
}