using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.Infrastructure.Subscription;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Cursus.Tests.TestSubscription
{
    [TestFixture]
    public class SubscriptionRepositoryTests
    {
        private Mock<CursusDBContext> _mockDbContext;
        private SubscriptionRepository _subscriptionRepository;

        [SetUp]
        public void SetUp()
        {
            _mockDbContext = new Mock<CursusDBContext>();
            _subscriptionRepository = new SubscriptionRepository(_mockDbContext.Object);
        }

        [Test]
        public void TestListSubscription()
        {
            // Arrange
            var accountId = 1;
            var userId = "user123";
            var subscribedCourses = new List<Subscribe>
            {
                new Subscribe { InstructorId = "instructor123", UserId = userId }
            };
            var instructors = new List<Account>
            {
                new Account { Id = "instructor123", FullName = "John Doe" }
            };

            var mockSetSubs = new Mock<DbSet<Subscribe>>();
            mockSetSubs.As<IQueryable<Subscribe>>().Setup(m => m.Provider).Returns(subscribedCourses.AsQueryable().Provider);
            mockSetSubs.As<IQueryable<Subscribe>>().Setup(m => m.Expression).Returns(subscribedCourses.AsQueryable().Expression);
            mockSetSubs.As<IQueryable<Subscribe>>().Setup(m => m.ElementType).Returns(subscribedCourses.AsQueryable().ElementType);
            mockSetSubs.As<IQueryable<Subscribe>>().Setup(m => m.GetEnumerator()).Returns(subscribedCourses.AsQueryable().GetEnumerator());

            var mockSetAccounts = new Mock<DbSet<Account>>();
            mockSetAccounts.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(instructors.AsQueryable().Provider);
            mockSetAccounts.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(instructors.AsQueryable().Expression);
            mockSetAccounts.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(instructors.AsQueryable().ElementType);
            mockSetAccounts.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(instructors.AsQueryable().GetEnumerator());

            _mockDbContext.Setup(db => db.Subscribes).Returns(mockSetSubs.Object);
            _mockDbContext.Setup(db => db.Accounts).Returns(mockSetAccounts.Object);

            var result = _subscriptionRepository.GetData(accountId, userId);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.SubscribeInstructor.Count);
            Assert.AreEqual("John Doe", result.SubscribeInstructor.First().FullName);
        }

        [Test]
        public void TestListSubscriptionNull()
        {
            var accountId = 1;
            var userId = "user123";
            List<Subscribe> subscribedCourses = null;
            var instructors = new List<Account>
            {
                new Account { Id = "instructor123", FullName = "John Doe" }
            };

            var mockSetSubs = new Mock<DbSet<Subscribe>>();
            mockSetSubs.As<IQueryable<Subscribe>>().Setup(m => m.Provider).Returns(subscribedCourses?.AsQueryable().Provider ?? new List<Subscribe>().AsQueryable().Provider);
            mockSetSubs.As<IQueryable<Subscribe>>().Setup(m => m.Expression).Returns(subscribedCourses?.AsQueryable().Expression ?? new List<Subscribe>().AsQueryable().Expression);
            mockSetSubs.As<IQueryable<Subscribe>>().Setup(m => m.ElementType).Returns(subscribedCourses?.AsQueryable().ElementType ?? new List<Subscribe>().AsQueryable().ElementType);
            mockSetSubs.As<IQueryable<Subscribe>>().Setup(m => m.GetEnumerator()).Returns(subscribedCourses?.AsQueryable().GetEnumerator() ?? new List<Subscribe>().AsQueryable().GetEnumerator());

            var mockSetAccounts = new Mock<DbSet<Account>>();
            mockSetAccounts.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(instructors.AsQueryable().Provider);
            mockSetAccounts.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(instructors.AsQueryable().Expression);
            mockSetAccounts.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(instructors.AsQueryable().ElementType);
            mockSetAccounts.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(instructors.AsQueryable().GetEnumerator());

            _mockDbContext.Setup(db => db.Subscribes).Returns(mockSetSubs.Object);
            _mockDbContext.Setup(db => db.Accounts).Returns(mockSetAccounts.Object);

            var result = _subscriptionRepository.GetData(accountId, userId);
            Assert.NotNull(result);
            Assert.AreEqual(0, result.SubscribeInstructor.Count);
            
        }
       
    }
}
