using System;
using System.Collections.Generic;

namespace Cursus.Domain.ViewModels
{
    public class InstructorPayoutsViewModel
    {
        // Overview metrics
        public decimal TotalPendingPayouts { get; set; }
        public decimal TotalPaidOut { get; set; }
        public decimal CurrentMonthPayouts { get; set; }
        public int PendingPayoutCount { get; set; }
        public int CompletedPayoutCount { get; set; }
        
        // Payout lists
        public List<PayoutRequest> PendingPayouts { get; set; } = new List<PayoutRequest>();
        public List<PayoutHistory> PayoutHistory { get; set; } = new List<PayoutHistory>();
        
        // Instructor earnings summary
        public List<InstructorEarningsSummary> InstructorEarnings { get; set; } = new List<InstructorEarningsSummary>();
        
        // Statistics
        public decimal AveragePayoutAmount { get; set; }
        public TimeSpan AverageProcessingTime { get; set; }
        public int TotalActiveInstructors { get; set; }
        
        // Date range for filtering
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        // Filter options
        public PayoutStatus? StatusFilter { get; set; }
        public string InstructorNameFilter { get; set; }
    }
    
    public class PayoutRequest
    {
        public int PayoutId { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public decimal Amount { get; set; }
        public PayoutStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentDetails { get; set; }
        public string Notes { get; set; }
        public List<PayoutTransaction> Transactions { get; set; } = new List<PayoutTransaction>();
        
        // Calculated fields
        public TimeSpan? ProcessingTime => ProcessedDate?.Subtract(RequestDate);
        public bool IsOverdue => Status == PayoutStatus.Pending && 
                                DateTime.Now.Subtract(RequestDate).Days > 7;
    }
    
    public class PayoutHistory
    {
        public int PayoutId { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        public decimal Amount { get; set; }
        public PayoutStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionReference { get; set; }
        public string ProcessedBy { get; set; }
        public TimeSpan ProcessingTime { get; set; }
    }
    
    public class InstructorEarningsSummary
    {
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string Email { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal PaidOut { get; set; }
        public decimal PendingPayout { get; set; }
        public decimal AvailableBalance { get; set; }
        public int CoursesCount { get; set; }
        public int TotalStudents { get; set; }
        public DateTime LastPayoutDate { get; set; }
        public decimal AverageMonthlyEarnings { get; set; }
        public List<CourseEarning> CourseEarnings { get; set; } = new List<CourseEarning>();
    }
    
    public class PayoutTransaction
    {
        public int TransactionId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string PaymentMethod { get; set; }
        public decimal InstructorShare { get; set; }
        public decimal PlatformFee { get; set; }
    }
    
    public class CourseEarning
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal InstructorEarnings { get; set; }
        public int EnrollmentCount { get; set; }
        public DateTime LastSaleDate { get; set; }
    }
    
    public enum PayoutStatus
    {
        Pending = 0,
        Processing = 1,
        Completed = 2,
        Failed = 3,
        Cancelled = 4
    }
}