using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application;
using Cursus.Application.DashBoard;
using Cursus.Application.Enroll;
using Cursus.Application.Subscrise;
using Cursus.Domain.Models;
using Cursus.Infrastructure;
using Cursus.Infrastructure.Enroll;
using Cursus.Infrastructure.Subscrise;
using Microsoft.EntityFrameworkCore;

namespace Cursus.Tests.DashboardTest
{
    public class InstructorDashboardTest
    {
        private DashBoardService _dashBoardService;
        private IEnrollRepository _enrollRepository;
        private ICourseRepository _courseRepository;
        private ISubscriseRepository _subscriseRepository;
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
            _subscriseRepository = new SubscriseRepository(_context);
            _dashBoardService = new DashBoardService(_enrollRepository, _courseRepository, _subscriseRepository);
        }

        [Test]
        public void TestGetData_AccountIdIsValid_ReturnsDashboardData()
        {
            // Arrange
            var accountId = 1;
            var id = "id";
            var course = new Course { AccountId = accountId, CourseId = 1, CourseName = "CourseName" };
            var enroll = new Enroll { AccountId = accountId, CourseId = 1 };
            var subscrise = new Subscribe { SubId = accountId };
            _context.Courses.Add(course);
            _context.Enrolls.Add(enroll);
            _context.Subscribes.Add(subscrise);
            _context.SaveChanges();

            // Act
            var dashboard = _dashBoardService.GetData(accountId, id);

            // Assert
            Assert.IsNotNull(dashboard, "GetData should return dashboard data");
            Assert.AreEqual(1, dashboard.TotalCourses, "TotalCourses should be 1");
            Assert.AreEqual(1, dashboard.TotalEnrolls, "TotalEnrolls should be 1");
            Assert.AreEqual(1, dashboard.TotalEnrolledUsers, "TotalEnrolledUsers should be 1");
            Assert.AreEqual(0, dashboard.TotalSubscrise, "TotalSubscrise should be 1");
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