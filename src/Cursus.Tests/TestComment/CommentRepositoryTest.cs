using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.Infrastructure;
using Cursus.Infrastructure.Comment;
using Microsoft.EntityFrameworkCore;

namespace Cursus.Tests.TestComment
{
	public class CommentRepositoryTest
	{
		private CommentReposidtory _commentRepository;
		private CursusDBContext _context { get; set; } = null!;
		private Commnent _comment;
		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<CursusDBContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			_context = new CursusDBContext(options);
			_commentRepository = new CommentReposidtory(_context);
			_comment = new Commnent
			{
				CmtId = 1,
				AccountId = 1,
				LessionId = 1,
				CmtContent = "CmtContent",
				CmtLevel = 1,
				CmtReply = "CmtReply",
				CmtDate = DateTime.Now
			};
			_context.Commnents.Add(_comment);
			_context.SaveChanges();

		}

		[Test]
		public void TestAddComment_Comment()
		{
			var comment = new Commnent
			{
				CmtId = 2,
				AccountId = 2,
				LessionId = 2,
				CmtContent = "CmtContent2",
				CmtLevel = 2,
				CmtReply = "CmtReply2",
				CmtDate = DateTime.Now
			};
			var result = _commentRepository.addComment(comment);
			Assert.AreEqual(comment, result);
		}

		[Test]
		public void TestAddComment_NullComment()
		{
			var result = _commentRepository.addComment(null);
			Assert.IsNull(result);
		}

		[Test]
		public void TestGetCommnentsByLessonID()
		{
			var result = _commentRepository.GetCommnentsByLessonID(1);
			Assert.AreEqual(1, result.Count);
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
