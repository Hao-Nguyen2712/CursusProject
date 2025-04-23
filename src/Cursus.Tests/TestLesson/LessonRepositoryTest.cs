using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Cursus.Tests.TestLesson
{
	public class LessonRepositoryTest
	{
		private LessonRepository _lessonRepository;
		private CursusDBContext _context { get; set; } = null!;
		private Lesson _lesson;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<CursusDBContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			_context = new CursusDBContext(options);
			_lessonRepository = new LessonRepository(_context);
			_lesson = new Lesson
			{
				LessionId = 1,
				CourseId = 1,
				LessionTilte = "LessonTile",
				LessionType = "Video",
				LessionVideo = "https://www.youtube.com/",
				LessionContent = "LessonContent",
				LessionComments = 0,
				LessionFinish = 0,
				LessionImage = "https://www.image.com/"
			};

			_context.Lessons.Add(_lesson);
			_context.SaveChanges();


		}

		[Test]
		public void TestAddLesson()
		{
			var lesson = new Lesson
			{
				LessionId = 2,
				CourseId = 2,
				LessionTilte = "LessonTile2",
				LessionType = "Video2",
				LessionVideo = "https://www.youtube.com/2",
				LessionContent = "LessonContent2",
				LessionComments = 0,
				LessionFinish = 0,
				LessionImage = "https://www.image.com/2"
			};

			var result = _lessonRepository.AddLesson(lesson);
			Assert.IsNotNull(result, "The lesson should be added successfully.");
			Assert.AreEqual(lesson.LessionTilte,result.LessionTilte, "The lesson name should match.");
		}

		[Test]
		public void TestAddLesson_NullLesson()
		{
			var result = _lessonRepository.AddLesson(null);
			Assert.IsNull(result, "The lesson should not be added.");
		}

		[Test]
		public void TestGetLesson()
		{
			var result = _lessonRepository.GetLesson(1);
			Assert.IsNotNull(result, "The lesson should be found.");
			Assert.AreEqual(_lesson.LessionTilte, result.LessionTilte, "The lesson name should match.");
		}

		[Test]
		public void TestGetLesson_NotFound()
		{
			var result = _lessonRepository.GetLesson(2);
			Assert.IsNull(result, "The lesson should not be found.");
		}

		[Test]
		public void TestUpadateLesson()
		{
			var lesson = new Lesson
			{
				LessionId = 1,
				CourseId = 1,
				LessionTilte = "LessonTile New",
				LessionType = "Video2",
				LessionVideo = "https://www.youtube.com/2",
				LessionContent = "LessonContent2",
				LessionComments = 0,
				LessionFinish = 0,
				LessionImage = "https://www.image.com/2"
			};
			var result = _lessonRepository.UpadateLesson(lesson);
			Assert.IsNotNull(result, "The lessons should be found.");
			Assert.AreEqual(lesson.LessionTilte, result.LessionTilte, "The number of lessons should match.");
		}

		[Test]
		public void TestUpadateLesson_NullLesson()
		{
			var result = _lessonRepository.UpadateLesson(null);
			Assert.IsNull(result, "The lesson should not be found.");
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
