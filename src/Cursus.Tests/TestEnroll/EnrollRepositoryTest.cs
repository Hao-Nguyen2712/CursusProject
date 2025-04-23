using Cursus.Domain.Models;
using Cursus.Infrastructure.Enroll;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Tests.TestEnroll
{
    public class EnrollRepositoryTest
    {
        private EnrollRepository _enrollRepository;
        private CursusDBContext _context { get; set; } = null!;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique name for each test
                .Options;

            _context = new CursusDBContext(options);
            _enrollRepository = new EnrollRepository(_context);
            _context.Enrolls.Add(new Domain.Models.Enroll
            {
                EnrollId = 1,
                CourseId = 89,
                AccountId = 1,
                EnrollBlock = "false",
                EnrollDate = DateTime.Now,
                EnrollStatus = "Enrolled",
                EnrollFinish = "2024-07-25"
            });
            _context.Enrolls.Add(new Domain.Models.Enroll
            {
                EnrollId = 2,
                CourseId = 90,
                AccountId = 2,
                EnrollBlock = "false",
                EnrollDate = DateTime.Now,
                EnrollStatus = "Enrolled",
                EnrollFinish = "2024-07-26"
            });
            _context.SaveChanges();
        }


        [Test]
        public void TestUnEnroll_EnrollExists()
        {
            // Arrange
            int courseId = 89;
            int accountId = 1;

            // Act
            var result = _enrollRepository.UnEnroll(courseId, accountId);

            // Assert
            Assert.IsTrue(result, "UnEnroll should return true when the enroll record exists.");

            var enroll = _context.Enrolls.FirstOrDefault(e => e.CourseId == courseId && e.AccountId == accountId);
            Assert.IsNotNull(enroll, "The enroll record should still exist after unenrolling.");
            Assert.AreEqual("Purchased", enroll.EnrollStatus, "The enroll status should be updated to 'Purchased'.");
            Assert.AreEqual("1900-1-1", enroll.EnrollFinish, "The enroll finish date should be updated to '1900-1-1'.");
        }

        [Test]
        public void TestUnEnroll_EnrollNotExists()
        {
            // Arrange
            int courseId = 999; // Course ID không tồn tại
            int accountId = 999; // Account ID không tồn tại

            // Act
            var result = _enrollRepository.UnEnroll(courseId, accountId);

            // Assert
            Assert.IsFalse(result, "UnEnroll should return false when the enroll record does not exist.");
        }
        [Test]
        public void TestUnEnroll_EnrollNotMatch()
        {
            // Arrange
            int courseId = 90;
            int accountId = 1; // Không có enroll nào khớp với cả CourseId 102 và AccountId 2

            // Act
            var result = _enrollRepository.UnEnroll(courseId, accountId);

            // Assert
            Assert.IsFalse(result, "UnEnroll should return false when no matching enroll record is found.");
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
