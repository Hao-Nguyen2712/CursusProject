using Cursus.Application;
using Cursus.Domain.Models;
using Cursus.Infrastructure;
using Cursus.Infrastructure.Enroll;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace Cursus.Tests.EnrollTest
{
    public class EnrollRepositoryTest
    {
        private EnrollRepository _enrollRepository;
        private CursusDBContext _context { get; set; } = null!;
        private Enroll _enroll;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CursusDBContext(options);
            _enrollRepository = new EnrollRepository(_context);

            _enroll = new Enroll
            {
                EnrollId = 1,
                CourseId = 1,
                AccountId = 1,
                EnrollFinish = "2025-05-06",
                EnrollDate = DateTime.Now,
                EnrollBlock = "false",
                EnrollStatus = "Purchased"
            };

            _context.Enrolls.Add(_enroll);
            _context.SaveChanges();
        }

        [Test]
        public void TestEnrollCourse()
        {
            // Arrange
            var courseId = 2; // A new course ID different from the existing one
            var accountId = 1; 

            // Act
            _enrollRepository.EnrollCourse(courseId, accountId);

            // Assert
            var result = _enrollRepository.GetEnrollByAccoutIDAndCourseID(accountId, courseId);
            Assert.IsNotNull(result, "The course should be enrolled successfully.");
            Assert.AreEqual("Enrolled", result.EnrollStatus, "The enroll status should be 'Enrolled'.");
        }

        [Test]
        public void TestEnrollCourse_ExistingEnrollment()
        {
            // Arrange
            var courseId = 1; 
            var accountId = 1; 

            // Act
            _enrollRepository.EnrollCourse(courseId, accountId);

            // Assert
            var result = _enrollRepository.GetEnrollByAccoutIDAndCourseID(accountId, courseId);
            Assert.IsNotNull(result, "The course should be enrolled successfully.");
            Assert.AreEqual("Enrolled", result.EnrollStatus, "The enroll status should be 'Enrolled'.");
        }

        [Test]
        public void TestEnrollCourseFree_NewEnrollment()
        {
            // Arrange
            var newCourseId = 3; 
            var accountId = 1; 

            // Act
            _enrollRepository.EnrollCourseFree(newCourseId, accountId);

            // Assert
            var result = _enrollRepository.GetEnrollByAccoutIDAndCourseID(accountId, newCourseId);
            Assert.IsNotNull(result, "The course should be enrolled successfully.");
            Assert.AreEqual("Enrolled", result.EnrollStatus, "The enroll status should be 'Enrolled'.");
        }

        [Test]
        public void TestEnrollCourseFree_ExistingEnrollment()
        {
            // Arrange
            var courseId = 1;
            var accountId = 1;

            _enrollRepository.EnrollCourseFree(courseId, accountId);

            // Assert
            var result = _enrollRepository.GetEnrollByAccoutIDAndCourseID(accountId, courseId);
            Assert.IsNotNull(result, "The course should be enrolled successfully.");
            Assert.AreEqual("Enrolled", result.EnrollStatus, "The enroll status should be 'Enrolled'.");
        }

        [Test]
        public void TestEnrollCourse_NullAccount()
        {
            // Arrange
            int newCourseId = 2;
            int accountId = -1; // An invalid account ID

            _enrollRepository.EnrollCourse(newCourseId, accountId);

            // Assert
            var result = _enrollRepository.GetEnrollByAccoutIDAndCourseID(accountId, newCourseId);
            Assert.IsNull(result, "The enroll should not be created for an invalid account.");
        }

        [Test]
        public void TestEnrollCourse_NullCourse()
        {
            // Arrange
            int courseId = 2;
            int accountId = 1;


            _enrollRepository.EnrollCourse(0, accountId); 

            // Assert
            var result = _enrollRepository.GetEnrollByAccoutIDAndCourseID(accountId, courseId);
            Assert.IsNull(result, "The enroll should not be created for a non-existent course.");
        }

        [Test]
        public void TestUnEnrollCourse()
        {
            // Arrange
            var courseId = 1;
            var accountId = 1;

            var result = _enrollRepository.UnEnroll(courseId, accountId);

            // Assert
            Assert.IsTrue(result, "The course should be un-enrolled successfully.");
            var enroll = _enrollRepository.GetEnrollByAccoutIDAndCourseID(accountId, courseId);
            Assert.AreEqual("Purchased", enroll.EnrollStatus, "The enroll status should be 'Purchased'.");
            Assert.AreEqual("1900-1-1", enroll.EnrollFinish, "The enroll finish date should be reset to '1900-1-1'.");
        }

        [Test]
        public void TestGetEnrollStatus()
        {
            // Arrange
            var courseId = 1;
            var accountId = 1;

            // Act
            var status = _enrollRepository.GetEnrollStatus(courseId, accountId);

            // Assert
            Assert.AreEqual("Purchased", status, "The enroll status should be 'Purchased'.");
        }

        [Test]
        public void TestGetEnrollStatus_Null()
        {
            // Arrange
            var courseId = 2; 
            var accountId = 1;

            // Act
            var status = _enrollRepository.GetEnrollStatus(courseId, accountId);

            // Assert
            Assert.AreEqual("Null", status, "The enroll status should be 'Null' for non-existent enrollments.");
        }

        [Test]
        public void TestDeleteProgressByCourseId_NoProgress()
        {
            // Arrange
            var courseId = 2; 

            // Act
            var result = _enrollRepository.DeleteProgressByCourseId(courseId);

            // Assert
            Assert.IsTrue(result, "No progress records should result in a successful deletion call.");
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
