using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Cursus.Tests.TestCourse
{
	public class CourseRepositoryTest
	{
		private CourseRepository _courseRepository;
		private CursusDBContext _context { get; set; } = null;
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
				CourseId = 3,
				AccountId = 1,
				CourseName = "CourseName",
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

            
			// Add test data
            _context.Courses.AddRange(new List<Course>
            {
                new Course
                {
                    CourseId = 1,
                    AccountId = 1,
                    CourseName = "Course 1",
                    CourseShortDes = "Short Des 1",
                    CourseDescription = "Description 1",
                    CourseWlearn = "Learn 1",
                    CourseRequirement = "Requirement 1",
                    CourseAvatar = "Avatar 1",
                    CourseDate = DateTime.Now,
                    CourseMoney = 100,
                    CourseStatus = "Active",
                    CourseProcess = 1,
                    Discount = 0,
                    CategoryId = 1
                },
                new Course
                {
                    CourseId = 4,
                    AccountId = 2,
                    CourseName = "Course 4",
                    CourseShortDes = "Short Des 2",
                    CourseDescription = "Description 2",
                    CourseWlearn = "Learn 2",
                    CourseRequirement = "Requirement 2",
                    CourseAvatar = "Avatar 2",
                    CourseDate = DateTime.Now,
                    CourseMoney = 200,
                    CourseStatus = "Pending",
                    CourseProcess = 2,
                    Discount = 10,
                    CategoryId = 2
                }
            });
            
            _context.Courses.AddRange();
			_context.Courses.Add(_course);
			_context.SaveChanges();
		}

		[Test]
		public void TestAddCourse()
		{
			Course course1 = new Course()
			{
				CourseId = 2,
				AccountId = 1,
				CourseName = "CourseName2",
				CourseShortDes = "CourseShortDes2",
				CourseDescription = "CourseDescription2",
				CourseWlearn = "CourseWlearn",
				CourseRequirement = "CourseRequirement2",
				CourseAvatar = "CourseAvatar2",
				CourseDate = DateTime.Now,
				CourseMoney = 100,
				CourseStatus = "CourseStatus2",
				CourseProcess = 1,
				Discount = 0,
				CategoryId = 1
			};
			var result = _courseRepository.AddCourse(course1);

			Assert.IsNotNull(result, "The course should be added successfully.");
			Assert.AreEqual(course1.CourseName, result.CourseName, "The course name should match.");
		}

		[Test]
		public void TestAddCourse_NullCourse()
		{
			var result = _courseRepository.AddCourse(null);
			Assert.IsNull(result, "Adding a null course should return null.");
		}

		//test GetAllCourse And GetCourseDetail
		public void TestGetAllCourse()
        {
            // Act
            var result = _courseRepository.GetAllCourse().ToList();

            // Assert
            Assert.IsNotNull(result, "The result should not be null.");
            Assert.AreEqual(2, result.Count, "There should be 2 courses in the database.");

            // Fetch expected courses from the context for validation
            var expectedCourses = _context.Courses.ToList();
            Assert.IsTrue(result.Any(c => c.CourseId == expectedCourses.First(c => c.CourseName == "Course 1").CourseId), "Course with ID of 'Course 1' should be present.");
            Assert.IsTrue(result.Any(c => c.CourseId == expectedCourses.First(c => c.CourseName == "Course 4").CourseId), "Course with ID of 'Course 4' should be present.");
        }

        [Test]
        public void TestGetCourseDetail()
        {
            // Fetch a course from the context for validation
            var expectedCourse = _context.Courses.FirstOrDefault(c => c.CourseId == 1);

            // Act
            var result = _courseRepository.GetCourseDetail(expectedCourse?.CourseId ?? 0);

            // Assert
            Assert.IsNotNull(result, "The course should be found.");
            Assert.AreEqual(expectedCourse?.CourseId, result.CourseId, "The course ID should match.");
            Assert.AreEqual(expectedCourse?.CourseName, result.CourseName, "The course name should match.");
        }

        [Test]
        public void TestGetCourseDetail_NotFound()
        {
            // Act
            var result = _courseRepository.GetCourseDetail(999); // ID không tồn tại

            // Assert
            Assert.IsNull(result, "The course should not be found.");
        }

        [Test]
        public void TestGetAllCourse_NoCourses()
        {
            // Arrange
            _context.Courses.RemoveRange(_context.Courses);
            _context.SaveChanges();

            // Act
            var result = _courseRepository.GetAllCourse().ToList();

            // Assert
            Assert.IsNotNull(result, "The result should not be null.");
            Assert.IsEmpty(result, "There should be no courses in the database.");
        }
		//End Test GetAllCourse And GetCourseDetail
		[Test]
		public void TestGetCourseDetail_CourseID()
		{
			int idCourse = 1;
			var result = _courseRepository.GetCourseDetail(idCourse);
			Assert.IsNotNull(result, "The course should not be found.");
		}

		[Test]
		public void TestGetCourseDetail_CourseID_NotFound()
		{
			int idCourse = 100;
			var result = _courseRepository.GetCourseDetail(idCourse);
			Assert.IsNull(result, "The course should not be found.");
		}

		[Test]
		public void TestGetCourseDetail_CourseID_Zero()
		{
			int idCourse = 0;
			var result = _courseRepository.GetCourseDetail(idCourse);
			Assert.IsNull(result, "The course should not be found.");
		}


		[TearDown]
		public void TearDown()
		{
			_context.Database.EnsureDeleted(); // Clean up the in-memory database
            _context.Dispose();
		}

		public void Dispose()
		{
			_context?.Dispose();
		}
	}
}
