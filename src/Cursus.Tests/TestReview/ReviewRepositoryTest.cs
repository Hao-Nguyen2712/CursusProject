using Cursus.Application.Models;
using Cursus.Domain.Models;
using Cursus.Infrastructure;
using Cursus.Infrastructure.Review;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cursus.Tests.TestReview
{
    public class ReviewRepositoryTest
    {
        private ReviewRepository _reviewRepository;
        private CursusDBContext _context { get; set; } = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CursusDBContext(options);
            _reviewRepository = new ReviewRepository(_context);

            // Add courses
            _context.Courses.Add(new Domain.Models.Course
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
                CourseStatus = "CourseStatus",
                CourseProcess = 1,
                Discount = 0,
                CategoryId = 1
            });

            // Add accounts
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

            // Add rates
            _context.Rates.Add(new Domain.Models.Rate
            {
                RateId = 1,
                CourseId = 1,
                AccountId = 1,
                RateContent = "Good course",
                RatePoint = 5,
                RateDate = DateTime.Now
            });

            _context.Rates.Add(new Domain.Models.Rate
            {
                RateId = 2,
                CourseId = 1,
                AccountId = 2,
                RateContent = "Average course",
                RatePoint = 3,
                RateDate = DateTime.Now
            });

            _context.SaveChanges();
        }

        [Test]
        public void TestGetReviewByCourseId_CourseExists()
        {
            // Arrange
            int courseId = 1;

            // Act
            var result = _reviewRepository.GetReviewByCourseId(courseId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null when course exists.");
            Assert.AreEqual(1, result[0].CourseId, "CourseId should match.");
            Assert.AreEqual("Course 1", result[0].CourseTitle, "CourseTitle should match.");
            Assert.AreEqual(2, result[0].TotalReviews, "TotalReviews should match.");
            Assert.AreEqual(4, result[0].AverageRatePoint, "AverageRatePoint should be correctly calculated.");
            Assert.AreEqual(50, result[0].Percent5Star, "Percent5Star should be correctly calculated.");
            Assert.AreEqual(0, result[0].Percent4Star, "Percent4Star should be correctly calculated.");
            Assert.AreEqual(50, result[0].Percent3Star, "Percent3Star should be correctly calculated.");
            Assert.AreEqual(0, result[0].Percent2Star, "Percent2Star should be correctly calculated.");
            Assert.AreEqual(0, result[0].Percent1Star, "Percent1Star should be correctly calculated.");
        }

        [Test]
        public void TestGetReviewByCourseId_CourseDoesNotExist()
        {
            // Arrange
            int courseId = 999; // Course ID không tồn tại

            // Act
            var result = _reviewRepository.GetReviewByCourseId(courseId);

            // Assert
            Assert.IsNull(result, "Result should be null when course does not exist.");
        }

        [Test]
        public void TestGetReviewByCourseId_NoReviews()
        {
            // Arrange
            int courseId = 2; // Course ID tồn tại nhưng không có review nào

            _context.Courses.Add(new Domain.Models.Course
            {
                CourseId = 2,
                AccountId = 2,
                CourseName = "Course 2",
                CourseShortDes = "CourseShortDes1",
                CourseDescription = "CourseDescription1",
                CourseWlearn = "CourseWlearn1",
                CourseRequirement = "CourseRequirement1",
                CourseAvatar = "CourseAvatar1",
                CourseDate = DateTime.Now,
                CourseMoney = 100,
                CourseStatus = "CourseStatus",
                CourseProcess = 1,
                Discount = 0,
                CategoryId = 1
            });

            _context.SaveChanges();

            // Act
            var result = _reviewRepository.GetReviewByCourseId(courseId);

            // Assert
            Assert.IsNull(result, "Result should be null even if there are no reviews.");
        }

        //[Test]
        //public void TestGetAllReview_UserExists()
        //{
        //    // Arrange
        //    string userId = "user1";

        //    // Act
        //    var result = _reviewRepository.GetAllReview(userId);

        //    // Assert
        //    Assert.IsNotNull(result, "Result should not be null when user exists.");
        //    Assert.AreEqual(1, result.Count, "Result should contain one courses.");
        //    Assert.AreEqual(1, result[0].CourseId, "First course should have ID 1.");
        //    Assert.AreEqual(2, result[0].Reviews.Count, "First course should have two reviews.");
        //    Assert.AreEqual(5, result[0].Reviews[0].RatePoint, "First review should have 5 stars.");
        //    Assert.AreEqual(3, result[0].Reviews[1].RatePoint, "Second review should have 3 stars.");
        //    Assert.AreEqual(4, result[0].AverageRatePoint, "Average rate point should be correctly calculated.");
        //}

        //[Test]
        //public void TestGetAllReview_UserDoesNotExist()
        //{
        //    // Arrange
        //    string userId = "nonexistent";

        //    // Act
        //    var result = _reviewRepository.GetAllReview(userId);

        //    // Assert
        //    Assert.IsNull(result, "Result should be null when user does not exist.");
        //}

        //[Test]
        //public void TestGetAllReview_NoReviews()
        //{
        //    // Arrange
        //    string userId = "user2"; // User có tài khoản nhưng không có khóa học nào

        //    _context.Courses.Add(new Domain.Models.Course
        //    {
        //        CourseId = 3,
        //        AccountId = 2,
        //        CourseName = "Course 3",
        //        CourseShortDes = "CourseShortDes1",
        //        CourseDescription = "CourseDescription1",
        //        CourseWlearn = "CourseWlearn1",
        //        CourseRequirement = "CourseRequirement1",
        //        CourseAvatar = "CourseAvatar1",
        //        CourseDate = DateTime.Now,
        //        CourseMoney = 100,
        //        CourseStatus = "CourseStatus",
        //        CourseProcess = 1,
        //        Discount = 0,
        //        CategoryId = 1
        //    });

        //    _context.SaveChanges();

        //    // Act
        //    var result = _reviewRepository.GetAllReview(userId);

        //    // Assert
        //    Assert.IsNull(result, "Result should be null even if there are no reviews.");
        //}

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
