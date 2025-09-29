using System;
using System.Collections.Generic;

namespace Cursus.Domain.ViewModels
{
    public class RevenueAnalyticsViewModel
    {
        // Overview metrics
        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public decimal DailyRevenue { get; set; }
        public decimal PreviousMonthRevenue { get; set; }
        public double MonthlyGrowthPercentage { get; set; }
        
        // Platform fees and instructor earnings
        public decimal PlatformFees { get; set; }
        public decimal InstructorEarnings { get; set; }
        public decimal PlatformFeePercentage { get; set; }
        
        // Course performance
        public List<TopSellingCourse> TopSellingCourses { get; set; } = new List<TopSellingCourse>();
        public int TotalCoursesSold { get; set; }
        public decimal AverageOrderValue { get; set; }
        
        // Payment method breakdown
        public List<PaymentMethodRevenue> PaymentMethodBreakdown { get; set; } = new List<PaymentMethodRevenue>();
        
        // Time-based analytics
        public List<MonthlyRevenueData> MonthlyRevenueData { get; set; } = new List<MonthlyRevenueData>();
        public List<DailyRevenueData> DailyRevenueData { get; set; } = new List<DailyRevenueData>();
        
        // Instructor-related metrics
        public int TotalActiveInstructors { get; set; }
        public List<TopEarningInstructor> TopEarningInstructors { get; set; } = new List<TopEarningInstructor>();
        
        // Date range for analytics
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    
    public class TopSellingCourse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseTitle { get; set; }
        public string InstructorName { get; set; }
        public int TotalSales { get; set; }
        public decimal CoursePrice { get; set; }
        public decimal TotalRevenue { get; set; }
        public DateTime LastSaleDate { get; set; }
    }
    
    public class PaymentMethodRevenue
    {
        public string PaymentMethod { get; set; }
        public decimal Revenue { get; set; }
        public int TransactionCount { get; set; }
        public double Percentage { get; set; }
    }
    
    public class MonthlyRevenueData
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public decimal Revenue { get; set; }
        public int TransactionCount { get; set; }
        public decimal PlatformFees { get; set; }
        public decimal InstructorEarnings { get; set; }
    }
    
    public class DailyRevenueData
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int TransactionCount { get; set; }
        public decimal PlatformFees { get; set; }
        public decimal InstructorEarnings { get; set; }
    }
    
    public class TopEarningInstructor
    {
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string Email { get; set; }
        public decimal TotalEarnings { get; set; }
        public int CoursesCount { get; set; }
        public int TotalStudents { get; set; }
        public decimal AverageRating { get; set; }
        public DateTime LastPayoutDate { get; set; }
    }
}