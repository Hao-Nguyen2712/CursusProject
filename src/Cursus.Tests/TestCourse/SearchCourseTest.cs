using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
namespace Cursus.Tests.TestCourse
{
    public class SearchCourseTest
    {
        private CourseRepository _courseRepository;
        private CursusDBContext _context { get; set; } = null!;
        private Course _course;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CursusDBContext(options);
            _courseRepository = new CourseRepository(_context);

            _course = new Course
            {
                CourseId = 1,
                AccountId = 1,
                CourseName = "TestCourse",
                CourseShortDes = "CourseShortDes",
                CourseDescription = "CourseDescription",
                CourseWlearn = "CourseWlearn",
                CourseRequirement = "CourseRequirement",
                CourseAvatar = "CourseAvatar",
                CourseDate = DateTime.Now,
                CourseMoney = 100,
                CourseStatus = "CourseStatus",
                CourseProcess = 1,
                Discount = 0,
                CategoryId = 1
            };

            _context.Courses.Add(_course);
            _context.SaveChanges();
        }

        [Test]
        public async Task TestGetCourseByNameAsync()
        {
            // Act
            var result = await _courseRepository.GetCourseByNameAsync("TestCourse");

            // Assert
            Assert.IsNotNull(result, "The result should not be null.");
            Assert.AreEqual(1, result.Count(), "The number of courses should match.");
            Assert.AreEqual("TestCourse", result.First().CourseName, "The course name should match.");
        }

        [Test]
        public async Task TestGetCourseByNameAsync_NotFound()
        {
            // Act
            var result = await _courseRepository.GetCourseByNameAsync("NonExistentCourse");

            // Assert
            Assert.IsNotNull(result, "The result should not be null.");
            Assert.AreEqual(0, result.Count(), "The number of courses should be zero.");
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