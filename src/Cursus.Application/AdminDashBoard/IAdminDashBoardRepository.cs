using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.ViewModels;
using Cursus.Domain.Models;

namespace Cursus.Application.AdminDashBoard
{
    public interface IAdminDashBoardRepository
    {
        Cursus.Domain.Models.AdminDashBoard GetData();
        
        // Revenue Analytics methods
        decimal GetTotalRevenue(DateTime? startDate = null, DateTime? endDate = null);
        decimal GetMonthlyRevenue(DateTime? month = null);
        List<TopSellingCourse> GetTopSellingCourses(int count = 10, DateTime? startDate = null, DateTime? endDate = null);
        List<PaymentMethodRevenue> GetPaymentMethodBreakdown(DateTime? startDate = null, DateTime? endDate = null);
        List<MonthlyRevenueData> GetMonthlyRevenueData(int months = 12);
        List<DailyRevenueData> GetDailyRevenueData(DateTime? startDate = null, DateTime? endDate = null);
        List<TopEarningInstructor> GetTopEarningInstructors(int count = 10, DateTime? startDate = null, DateTime? endDate = null);
        
        // Payout management methods
        List<PayoutRequest> GetPendingPayouts();
        List<PayoutHistory> GetPayoutHistory(DateTime? startDate = null, DateTime? endDate = null);
        List<InstructorEarningsSummary> GetInstructorEarningsSummary();
        Task<bool> UpdatePayoutStatusAsync(int payoutId, PayoutStatus status, string processedBy);
        Task<PayoutRequest> CreatePayoutRequestAsync(int instructorId, decimal amount, string paymentMethod);
        PayoutRequest GetPayoutById(int payoutId);
    }
}