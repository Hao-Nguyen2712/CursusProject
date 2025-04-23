using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.Subscrise;
using Cursus.Domain.Models;
using Cursus.Infrastructure.Subscrise;
using Microsoft.EntityFrameworkCore;

namespace Cursus.Tests.SubscribeTest
{
    public class SubscribeRepositoryTest
    {
        private SubscriseService _subscriseService;
        private ISubscriseRepository _subscriseRepository;
        private CursusDBContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CursusDBContext(options);
            _subscriseRepository = new SubscriseRepository(_context);
            _subscriseService = new SubscriseService(_subscriseRepository);
        }

        [Test]
        public void TestAddSub_AddsSubscribeToRepository()
        {
            // Arrange
            var sub = new Subscribe { UserId = "user1", InstructorId = "instructor1" };

            // Act
            _subscriseService.AddSub(sub);

            // Assert
            Assert.IsFalse(_subscriseRepository.GetSubById(sub.InstructorId).Any(s => s.UserId == sub.UserId));
        }

        [Test]
        public void TestGetSubByInstructorAndUserId_ReturnsSubscribe()
        {
            // Arrange
            var sub = new Subscribe { UserId = "user1", InstructorId = "instructor1" };
            _subscriseRepository.AddSub(sub);

            // Act
            var result = _subscriseService.GetSubByInstructorAndUserId(sub.InstructorId, sub.UserId);

            // Assert
            Assert.IsNotNull(result, "GetSubByInstructorAndUserId should return a subscribe");
            Assert.AreEqual(sub.UserId, result.UserId, "UserId should match");
            Assert.AreEqual(sub.InstructorId, result.InstructorId, "InstructorId should match");
        }

        [Test]
        public void TestUnSub_RemovesSubscribeFromRepository()
        {
            // Arrange
            var sub = new Subscribe { UserId = "user1", InstructorId = "instructor1" };
            _subscriseRepository.AddSub(sub);

            // Act
            _subscriseService.UnSub(sub.InstructorId, sub.UserId);

            // Assert
            Assert.IsFalse(_subscriseRepository.GetSubById(sub.InstructorId).Any(s => s.UserId == sub.UserId));
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