using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application;
using Cursus.Application.Account;
using Cursus.Application.AdminDashBoard;
using Cursus.Application.Enroll;
using Cursus.Application.Report;
using Cursus.Domain.Models;
using Cursus.Domain.ViewModels;
using Cursus.Infrastructure;
using Cursus.Infrastructure.Enroll;
using Cursus.Infrastructure.Report;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Cursus.Tests.DashboardTest
{
    public class AdminDashboardTest
    {
        private AdminDashBoardService _adminDashBoardService;
        private IEnrollRepository _enrollRepository;
        private ICourseRepository _courseRepository;
        private IReportRepository _reportRepository;
        private IAccountRepository _accountRepository;
        private Mock<IAdminDashBoardRepository> _mockAdminDashBoardRepository;
        private CursusDBContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CursusDBContext(options);
            _enrollRepository = new EnrollRepository(_context);
            _courseRepository = new CourseRepository(_context);
            _reportRepository = new ReportRepository(_context);
            _accountRepository = new AccountRepository(_context);
            _mockAdminDashBoardRepository = new Mock<IAdminDashBoardRepository>();
            _adminDashBoardService = new AdminDashBoardService(_enrollRepository, _accountRepository, _courseRepository, _reportRepository, _mockAdminDashBoardRepository.Object);
        }

        [Test]
        public void TestGetData_ReturnsAdminDashboardData()
        {
            // Arrange
            var enroll = new Enroll { Course = new Course { CourseMoney = 100 } };
            var course = new Course { CourseStatus = "Pending Approval" };
            var account = new Account { Description = "Instructor" };
            _context.Enrolls.Add(enroll);
            _context.Courses.Add(course);
            _context.Accounts.Add(account);
            _context.SaveChanges();

            // Act
            var adminDashboard = _adminDashBoardService.GetData();

            // Assert
            Assert.IsNotNull(adminDashboard, "GetData should return admin dashboard data");
            Assert.AreEqual(1, adminDashboard.TotalUsers, "TotalUsers should be 1");
            Assert.AreEqual(100, adminDashboard.TotalRevenue, "TotalRevenue should be 100");
            Assert.AreEqual(2, adminDashboard.TotalCourses, "TotalCourses should be 2");
            Assert.AreEqual(0, adminDashboard.PendingReports, "PendingReports should be 0");
            Assert.AreEqual(1, adminDashboard.PendingCourseList.Count, "PendingCourseList should have 1 course");
            Assert.AreEqual(1, adminDashboard.IntructorsList.Count, "IntructorsList should have 1 instructor");
        }

        [Test]
        public void TestGetData_NoData_ReturnsZeroValues()
        {
            // Act
            var adminDashboard = _adminDashBoardService.GetData();

            // Assert
            Assert.IsNotNull(adminDashboard, "GetData should return admin dashboard data");
            Assert.AreEqual(0, adminDashboard.TotalUsers, "TotalUsers should be 0");
            Assert.AreEqual(0, adminDashboard.TotalRevenue, "TotalRevenue should be 0");
            Assert.AreEqual(0, adminDashboard.TotalCourses, "TotalCourses should be 0");
            Assert.AreEqual(0, adminDashboard.PendingReports, "PendingReports should be 0");
            Assert.AreEqual(0, adminDashboard.PendingCourseList.Count, "PendingCourseList should be empty");
            Assert.AreEqual(0, adminDashboard.IntructorsList.Count, "IntructorsList should be empty");
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}