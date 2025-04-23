using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Cursus.Domain.Models;
using Cursus.Infrastructure.Category;
using Microsoft.EntityFrameworkCore;

namespace Cursus.Tests.TestCategory
{
    [TestFixture]
    public class CategoryRepositoryTests
    {
        private Mock<CursusDBContext> _mockDbContext;
        private CategoryRepository _categoryRepository;

        [SetUp]
        public void Setup()
        {
            _mockDbContext = new Mock<CursusDBContext>();
            _categoryRepository = new CategoryRepository(_mockDbContext.Object);
        }

        [Test]
        public void TestGetCategory()
        {
            var categoryId = 1;
            var courses = new List<Course>
            {
                new Course { CourseId = 1, CourseName = "Course 1", CategoryId = categoryId },
                new Course { CourseId = 2, CourseName = "Course 2", CategoryId = categoryId }
            };

            var mockSet = new Mock<DbSet<Course>>();
            mockSet.As<IQueryable<Course>>().Setup(m => m.Provider).Returns(courses.AsQueryable().Provider);
            mockSet.As<IQueryable<Course>>().Setup(m => m.Expression).Returns(courses.AsQueryable().Expression);
            mockSet.As<IQueryable<Course>>().Setup(m => m.ElementType).Returns(courses.AsQueryable().ElementType);
            mockSet.As<IQueryable<Course>>().Setup(m => m.GetEnumerator()).Returns(courses.AsQueryable().GetEnumerator());

            _mockDbContext.Setup(c => c.Courses).Returns(mockSet.Object);

            var result = _categoryRepository.GetCategory(categoryId);

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(c => c.CategoryId == categoryId));
        }
        [Test]
        public void TestGetCategory_NoCoursesForCategory()
        {
            var categoryId = 1;
            var courses = new List<Course>();

            var mockSet = new Mock<DbSet<Course>>();
            mockSet.As<IQueryable<Course>>().Setup(m => m.Provider).Returns(courses.AsQueryable().Provider);
            mockSet.As<IQueryable<Course>>().Setup(m => m.Expression).Returns(courses.AsQueryable().Expression);
            mockSet.As<IQueryable<Course>>().Setup(m => m.ElementType).Returns(courses.AsQueryable().ElementType);
            mockSet.As<IQueryable<Course>>().Setup(m => m.GetEnumerator()).Returns(courses.AsQueryable().GetEnumerator());

            _mockDbContext.Setup(c => c.Courses).Returns(mockSet.Object);

            var result = _categoryRepository.GetCategory(categoryId);

            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count);
        }

    }
}
