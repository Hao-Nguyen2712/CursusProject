using Cursus.Domain.Models;
using Cursus.Infrastructure.Analyze;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Tests.TestAnalyzeCourse
{
    public class AnalyzeCourseRepoTest
    {
        private AnalyzeRepository _analyzeRepository;
        private CursusDBContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CursusDBContext(options);
            _analyzeRepository = new AnalyzeRepository(_context);


            var courses = new List<Course>
            {
                new Course { CourseId = 1, CourseName = "Course 1" },
                new Course { CourseId = 2, CourseName = "Course 2" }
            };
            var enrolls = new List<Domain.Models.Enroll>
            {
                new Domain.Models.Enroll { EnrollId = 1, CourseId = 1, AccountId = 1 },
                new Domain.Models.Enroll { EnrollId = 2, CourseId = 2, AccountId = 2 }
            };
            _context.Courses.AddRange(courses);
            _context.Enrolls.AddRange(enrolls);
            _context.SaveChanges();
        }
        [Test]
        public void GetCourses()
        {
            // Act
            var courses = _analyzeRepository.GetCourses();

            // Assert
            Assert.IsNotNull(courses, "Have a value in this");
        }

        [Test]
        public void GetEnroll()
        {
            // Act
            var enrolls = _analyzeRepository.GetEnroll();

            // Assert
            Assert.IsNotNull(enrolls, "Have a value");
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
