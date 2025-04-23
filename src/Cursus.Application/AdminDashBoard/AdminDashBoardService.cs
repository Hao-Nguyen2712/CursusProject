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

namespace Cursus.Application.AdminDashBoard
{
    public class AdminDashBoardService : IAdminDashBoardService
    {
        private readonly IEnrollRepository _enrollRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IAccountRepository _accountRepository;
        public AdminDashBoardService(IEnrollRepository enrollRepository,IAccountRepository accountRepository, ICourseRepository courseRepository,IReportRepository reportRepository)
        {
            _enrollRepository = enrollRepository;
            _courseRepository = courseRepository;
            _reportRepository = reportRepository;
            _accountRepository = accountRepository;
        }
        public Domain.Models.AdminDashBoard GetData()
        {
            // Get all enrolls and courses for the given account ID
                // Get all enrolls and courses for the given account ID
    var enrolls = _enrollRepository.GetAllEnroll();
    var courses = _courseRepository.GetAllCourse();
    var accounts = _accountRepository.GetAllAccount(); // Assuming you have an account repository
    var reports = _reportRepository.GetAllReport(); // Assuming you have a report repository

    // Calculate Admindashboard data
    var Admindashboard = new Domain.Models.AdminDashBoard()
    {
        TotalUsers = accounts.Count(),
        TotalRevenue = (decimal)enrolls.Sum(e => e.Course.CourseMoney),
        TotalCourses = courses.Count(),
        PendingReports = reports.Count(), // Assuming you have a Status property in the report model
        PendingCourseList = courses.Where(c => c.CourseStatus == "Pending Approval").ToList(), // Assuming you have a Status property in the course model
        IntructorsList = accounts.Where(a => a.Description == "Instructor").ToList() // Assuming you have a role property in the account model
    };

    return Admindashboard;
        }
    }
}