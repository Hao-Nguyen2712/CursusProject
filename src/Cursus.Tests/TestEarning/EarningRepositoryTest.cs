using Cursus.Application.Earning;
using Cursus.Domain.Models;
using Cursus.Infrastructure.Credits;
using Cursus.Infrastructure.Earning;
using Cursus.Infrastructure.Enroll;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Tests.TestEarning
{
    public class EarningRepositoryTest
    {
        private EarningRepository _earningRepository;
        private CursusDBContext _context { get; set; } = null;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CursusDBContext(options);
            _earningRepository = new EarningRepository(_context);

            //Add account
            var account1 = new Account
            {
                AccountId = 1,
                Username = "Test",
                Email = "Test@gmail.com",
                Password = "Password",
                FullName = "User 1",
                Phone = "0912345678",
                Gender = "Male",
                DateofBirth = new DateTime(1990, 1, 1), // Use a specific date for consistency
                Description = "Test",
                Avatar = "Avatar1.png",
                Role = 3,
                Money = 100, // Ensure Money is not null initially
                UpLevel = "false",
                IsDelete = "false",
                Id = "user1",
            };
            var account2 = new Account
            {
                AccountId = 2,
                Username = "Test",
                Email = "Test@gmail.com",
                Password = "Password",
                FullName = "User 2",
                Phone = "0912345678",
                Gender = "Male",
                DateofBirth = new DateTime(1990, 1, 1), // Use a specific date for consistency
                Description = "Test",
                Avatar = "Avatar2.png",
                Role = 3,
                Money = 100, // Ensure Money is not null initially
                UpLevel = "false",
                IsDelete = "false",
                Id = "user2",
            };
            _context.Accounts.AddRange(account1, account2);

            var course1 = new Domain.Models.Course
            {
                CourseId = 1,
                AccountId = 1,
                CourseName = "Course 1",
                CourseShortDes = "CourseShortDes1",
                CourseDescription = "CourseDescription1",
                CourseWlearn = "CourseWlearn1",
                CourseRequirement = "CourseRequirement1",
                CourseAvatar = "CourseAvatar1",
                CourseDate = DateTime.Now,
                CourseMoney = 100,
                CourseStatus = "CourseStatus1",
                CourseProcess = 1,
                Discount = 0,
                CategoryId = 1
            };
            var course2 = new Domain.Models.Course
            {
                CourseId = 2,
                AccountId = 1,
                CourseName = "Course 2",
                CourseShortDes = "CourseShortDes2",
                CourseDescription = "CourseDescription2",
                CourseWlearn = "CourseWlearn2",
                CourseRequirement = "CourseRequirement2",
                CourseAvatar = "CourseAvatar2",
                CourseDate = DateTime.Now,
                CourseMoney = 200,
                CourseStatus = "CourseStatus2",
                CourseProcess = 1,
                Discount = 0,
                CategoryId = 1
            };
            _context.Courses.AddRange(course1, course2);

            var enroll1 = new Domain.Models.Enroll
            {
                EnrollId = 1,
                CourseId = 1,
                AccountId = 1,
                EnrollBlock = "false",
                EnrollDate = DateTime.Now,
                EnrollStatus = "Enrolled",
                EnrollFinish = "2024-07-25"
            };
            var enroll2 = new Domain.Models.Enroll
            {
                EnrollId = 2,
                CourseId = 1,
                AccountId = 2,
                EnrollBlock = "false",
                EnrollDate = DateTime.Now,
                EnrollStatus = "Enrolled",
                EnrollFinish = "2024-07-26"
            };
            var enroll3 = new Domain.Models.Enroll
            {
                EnrollId = 3,
                CourseId = 2,
                AccountId = 1,
                EnrollBlock = "false",
                EnrollDate = DateTime.Now,
                EnrollStatus = "Enrolled",
                EnrollFinish = "2024-07-26"
            };
            _context.Enrolls.AddRange(enroll1, enroll2, enroll3);
            _context.SaveChanges();
        }
        [Test]
        public void TestGetTop10Courses_UserExists()
        {
            // Arrange
            string userId = "user1";

            // Act
            var result = _earningRepository.GetTop10Courses(userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null when user exists.");
            Assert.AreEqual(2, result.Count, "Result should contain two courses.");
            Assert.AreEqual("Course 1", result[0], "First course should be 'Course 1'.");
            Assert.AreEqual("Course 2", result[1], "Second course should be 'Course 2'.");
        }

        [Test]
        public void TestGetTop10Courses_UserDoesNotExist()
        {
            // Arrange
            string userId = "nonexistent";

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _earningRepository.GetTop10Courses(userId));
            Assert.AreEqual("Không tìm thấy thông tin tài khoản.", ex.Message);
        }

        [Test]
        public void TestGetTop10Courses_NoCourses()
        {
            // Arrange
            string userId = "user2"; // User có tài khoản nhưng không có khóa học nào

            // Act
            var result = _earningRepository.GetTop10Courses(userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null even if there are no courses.");
            Assert.AreEqual(0, result.Count, "Result should be empty when there are no courses.");
        }

        [Test]
        public void TestGetTop10EnrollCounts_UserExists()
        {
            // Arrange
            string userId = "user1";

            // Act
            var result = _earningRepository.GetTop10EnrollCounts(userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null when user exists.");
            Assert.AreEqual(2, result.Count, "Result should contain two enroll counts.");
            Assert.AreEqual("2", result[0], "First enroll count should be '2'.");
            Assert.AreEqual("1", result[1], "Second enroll count should be '1'.");
        }

        [Test]
        public void TestGetTop10EnrollCounts_UserDoesNotExist()
        {
            // Arrange
            string userId = "nonexistent";

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _earningRepository.GetTop10EnrollCounts(userId));
            Assert.AreEqual("Không tìm thấy thông tin tài khoản.", ex.Message);
        }

        [Test]
        public void TestGetTop10EnrollCounts_NoEnrolls()
        {
            // Arrange
            string userId = "user2"; // User có tài khoản nhưng không có enroll nào

            // Act
            var result = _earningRepository.GetTop10EnrollCounts(userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null even if there are no enrolls.");
            Assert.AreEqual(0, result.Count, "Result should be empty when there are no enrolls.");
        }

        [Test]
        public void TestGetTopSellingCourses_UserExists()
        {
            // Arrange
            string userId = "user1";

            // Act
            var result = _earningRepository.GetTopSellingCourses(userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null when user exists.");
            Assert.AreEqual(2, result.Count, "Result should contain two courses.");
            Assert.AreEqual("Course 1", result[0].CourseTitle, "First course title should be 'Course 1'.");
            Assert.AreEqual("Course 2", result[1].CourseTitle, "Second course title should be 'Course 2'.");
        }

        [Test]
        public void TestGetTopSellingCourses_UserDoesNotExist()
        {
            // Arrange
            string userId = "nonexistent";

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _earningRepository.GetTopSellingCourses(userId));
            Assert.AreEqual("Không tìm thấy thông tin tài khoản.", ex.Message);
        }

        [Test]
        public void TestGetTotalRevenueForCurrentMonth_UserExists()
        {
            // Arrange
            string userId = "user1";
            var expectedTotalRevenue = _context.Enrolls
                .Where(e => e.EnrollDate.HasValue
                            && e.EnrollDate.Value.Month == DateTime.Now.Month
                            && e.EnrollDate.Value.Year == DateTime.Now.Year
                            && e.Course.AccountId == 1)
                .Sum(e => e.Course.CourseMoney);

            // Act
            var result = _earningRepository.GetTotalRevenueForCurrentMonth(userId);

            // Assert
            Assert.AreEqual(expectedTotalRevenue, result, "Total revenue should match the expected value.");
        }

        [Test]
        public void TestGetTotalRevenueForCurrentMonth_UserDoesNotExist()
        {
            // Arrange
            string userId = "nonexistent";

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _earningRepository.GetTotalRevenueForCurrentMonth(userId));
            Assert.AreEqual("Không tìm thấy thông tin tài khoản.", ex.Message);
        }

        [Test]
        public void TestTotalIncomebyDay_UserExists()
        {
            // Arrange
            string userId = "user1";
            var expectedTotalIncome = _context.Enrolls
                .Where(e => e.EnrollDate.HasValue
                            && e.EnrollDate.Value.Date == DateTime.Today
                            && e.Course.AccountId == 1)
                .Sum(e => e.Course.CourseMoney);

            // Act
            var result = _earningRepository.TotalIncomebyDay(userId);

            // Assert
            Assert.AreEqual(expectedTotalIncome, result, "Total income should match the expected value.");
        }

        [Test]
        public void TestTotalIncomebyDay_UserDoesNotExist()
        {
            // Arrange
            string userId = "nonexistent";

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _earningRepository.TotalIncomebyDay(userId));
            Assert.AreEqual("Không tìm thấy thông tin tài khoản.", ex.Message);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
