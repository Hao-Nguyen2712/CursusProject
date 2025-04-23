using Cursus.Application.Credits;
using Cursus.Domain.Models;
using Cursus.Infrastructure.Credits;
using Cursus.Infrastructure.Payout;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Tests.TestPayout
{
    public class PayoutRepositoryTest
    {
        private PayoutRepository _payoutRepository;
        private CursusDBContext _context { get; set; } = null!;
        private Account _account;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique name for each test
                .Options;

            _context = new CursusDBContext(options);
            _payoutRepository = new PayoutRepository(_context);
            var account1 = new Account
            {
                AccountId = 1,
                Username = "Test",
                Email = "Test@gmail.com",
                Password = "Password",
                FullName = "User 1",
                Phone = "0912345678",
                Gender = "Male",
                DateofBirth = new DateTime(1990, 1, 1), // Use a specific date for consistency
                Description = "Test",
                Avatar = "Test",
                Role = 3,
                Money = 100, // Ensure Money is not null initially
                UpLevel = "false",
                IsDelete = "false",
                Id = "user1",
            };
            var account2 = new Account
            {
                AccountId = 2,
                Username = "Test",
                Email = "Test@gmail.com",
                Password = "Password",
                FullName = "User 2",
                Phone = "0912345678",
                Gender = "Male",
                DateofBirth = new DateTime(1990, 1, 1), // Use a specific date for consistency
                Description = "Test",
                Avatar = "Test",
                Role = 3,
                Money = 100, // Ensure Money is not null initially
                UpLevel = "false",
                IsDelete = "false",
                Id = "user2",
            };
            _context.Accounts.AddRange(account1, account2);
            _context.Tradings.AddRange(
                new Trading { TdId = 1, TdMoney = 500, TdDate = DateTime.Now, TdMethodPayment = "Payout", AccountId = 1 },
                 new Trading { TdId = 2, TdMoney = 200, TdDate = DateTime.Now, TdMethodPayment = "Bank", AccountId = 1 },
                  new Trading { TdId = 3, TdMoney = 300, TdDate = DateTime.Now, TdMethodPayment = "Bank", AccountId = 2 }
                );
            _context.SaveChanges();
        }

        [Test]
        public void TestUpdateAccMoney_AccountExists()
        {
            // Arrange
            string userId = "user1";
            int amountToDeduct = 50;

            // Act
            var result = _payoutRepository.UpdateAccMoney(userId, amountToDeduct);

            // Assert
            Assert.IsNotNull(result, "The account should be returned for an existing user.");
            Assert.AreEqual(50, result.Money, "The account's money should be correctly deducted.");
        }

        [Test]
        public void TestUpdateAccMoney_AccountNotExists()
        {
            // Arrange
            string userId = "non_existing_user";
            int amountToDeduct = 50;

            // Act
            var result = _payoutRepository.UpdateAccMoney(userId, amountToDeduct);

            // Assert
            Assert.IsNull(result, "The account should be null if the user does not exist.");
        }
        [Test]
        public void TestUpdateAccMoney_AmountGreaterThanMoney()
        {
            // Arrange
            string userId = "user1";
            int amountToDeduct = 200; // More than the current money

            // Act
            var result = _payoutRepository.UpdateAccMoney(userId, amountToDeduct);

            // Assert
            Assert.IsNull(result, "The account should be null if the user does not exist.");
        }

        [Test]
        public void TestGetAllPayout_AccountExistsWithPayouts()
        {
            // Arrange
            string userId = "user1";

            // Act
            var result = _payoutRepository.GetAllPayout(userId);

            // Assert
            Assert.IsNotNull(result, "The returned list of tradings should not be null.");
            Assert.AreEqual(1, result.Count, "The number of tradings returned should be 1.");
            Assert.AreEqual("Payout", result.First().TdMethodPayment, "The method payment should be 'Payout'.");
        }

        [Test]
        public void TestGetAllPayout_AccountExistsWithoutPayouts()
        {
            // Arrange
            string userId = "user2"; // User 2 có một giao dịch Payout

            // Act
            var result = _payoutRepository.GetAllPayout(userId);

            // Assert
            Assert.IsNull(result, "The returned value should be null when the account does not exist.");
        }

        [Test]
        public void TestGetAllPayout_AccountNotExists()
        {
            // Arrange
            string userId = "non_existing_user"; // Tài khoản không tồn tại

            // Act
            var result = _payoutRepository.GetAllPayout(userId);

            // Assert
            Assert.IsNull(result, "The returned value should be null when the account does not exist.");
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
