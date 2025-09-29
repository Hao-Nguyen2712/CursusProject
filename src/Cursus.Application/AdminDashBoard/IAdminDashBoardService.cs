using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.ViewModels;

namespace Cursus.Application.AdminDashBoard
{
    public interface IAdminDashBoardService
    {
        Cursus.Domain.Models.AdminDashBoard GetData();
        RevenueAnalyticsViewModel GetRevenueAnalytics(DateTime? startDate = null, DateTime? endDate = null);
        InstructorPayoutsViewModel GetInstructorPayouts(DateTime? startDate = null, DateTime? endDate = null);
        Task<bool> ProcessPayoutAsync(int payoutId, string adminId);
        Task<bool> BulkProcessPayoutsAsync(List<int> payoutIds, string adminId);
        Task<PayoutRequest> CreatePayoutRequestAsync(int instructorId, decimal amount, string paymentMethod);
    }
}