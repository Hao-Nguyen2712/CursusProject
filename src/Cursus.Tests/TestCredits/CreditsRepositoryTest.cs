using Cursus.Application.Account;
using Cursus.Domain.Models;
using Cursus.Infrastructure;
using Cursus.Infrastructure.Credits;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace Cursus.Tests.TestCredits
{
    public class CreditsRepositoryTest
    {
        private CreditsRepository _creditsRepository;
        private CursusDBContext _context { get; set; } = null!;
        private Account _account;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique name for each test
                .Options;

            _context = new CursusDBContext(options);
            _creditsRepository = new CreditsRepository(_context);

            var account1 = new Account {
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
                new Trading { TdId = 1, TdMoney = 500, TdDate = DateTime.Now, TdMethodPayment = "Bank", AccountId = 1 },
                 new Trading { TdId = 2, TdMoney = 200, TdDate = DateTime.Now, TdMethodPayment = "Bank", AccountId = 1 },
                  new Trading { TdId = 3, TdMoney = 300, TdDate = DateTime.Now, TdMethodPayment = "Payout", AccountId = 2 }
                );
            _context.SaveChanges();
        }

        [Test]
        public void TestAddAccMoney_AccountExists()
        {
            // Arrange
            string userId = "user1";
            int accMoneyToAdd = 50;

            // Act
            var result = _creditsRepository.AddAccMoney(userId, accMoneyToAdd);

            // Assert
            Assert.IsNotNull(result, "The returned account should not be null.");
            Assert.AreEqual(150, result.Money, "The account's money should be updated correctly.");
        }

        [Test]
        public void TestAddAccMoney_AccountNotExists()
        {
            // Arrange
            string userId = "non_existing_user";
            int accMoneyToAdd = 50;

            // Act
            var result = _creditsRepository.AddAccMoney(userId, accMoneyToAdd);

            // Assert
            Assert.IsNull(result, "Adding money to a non-existing account should return null.");
        }

        [Test]
        public void TestAddAccMoney_AccountMoneyIsNull()
        {
            // Arrange
            string userId = "user1";
            var account = _context.Accounts.First(a => a.Id == userId);
            account.Money = null; // Set Money to null
            _context.SaveChanges(); // Save changes to the in-memory database

            int accMoneyToAdd = 50;

            // Act
            var result = _creditsRepository.AddAccMoney(userId, accMoneyToAdd);

            // Assert
            Assert.IsNull(result, "The returned account should return null when Money is null.");
        }

        [Test]
        public void TestGetAccMoney()
        {
            // Arrange
            string userId = "user1";

            // Act
            var result = _creditsRepository.GetAccMoney(userId);

            // Assert
            Assert.IsNotNull(result, "The returned money value should not be null for an existing account.");
            Assert.AreEqual(100, result, "The account's money should be correctly retrieved as 100.");
        }

        [Test]
        public void TestGetAccMoney_AccountIsNull()
        {
            // Arrange
            string userId = "non_existing_user";

            // Act
            var result = _creditsRepository.GetAccMoney(userId);

            // Assert
            Assert.AreEqual(0, result, "The returned money value should be 0 for a non-existing account.");
        }

        [Test]
        public void TestAddTrading_ValidTradingAndAccountExists()
        {
            // Arrange
            string userId = "user1";
            var trading = new Trading
            {
                TdId = 4,
                TdMoney = 100,
                TdDate = DateTime.Now,
                TdMethodPayment = "Credit",
                AccountId = 1
            };

            // Act
            var result = _creditsRepository.AddTrading(trading, userId);

            // Assert
            Assert.IsNotNull(result, "The trading should be added successfully for an existing account.");
            Assert.AreEqual(trading.TdId, result.TdId, "The returned trading ID should match the input.");
            Assert.AreEqual(trading.TdMoney, result.TdMoney, "The returned trading amount should match the input.");
            Assert.AreEqual(1, result.AccountId, "The trading should be associated with the correct account.");
        }

        [Test]
        public void TestAddTrading_AccountNotExists()
        {
            // Arrange
            string userId = "non_existing_user";
            var trading = new Domain.Models.Trading
            {
                TdId = 1,
                TdMoney = 100,
                TdDate = DateTime.Now,
                TdMethodPayment = "Credit",
                AccountId = 1
            };

            // Act
            var result = _creditsRepository.AddTrading(trading, userId);

            // Assert
            Assert.IsNull(result, "The trading should not be added if the account does not exist.");
        }

        [Test]
        public void TestAddTrading_TradingIsNull()
        {
            // Arrange
            string userId = "user1";
            Trading trading = null;

            // Act
            var result = _creditsRepository.AddTrading(trading, userId);

            // Assert
            Assert.IsNull(result, "The method should return null if the trading object is null.");
        }

        [Test]
        public void TestGetAllTrading_AccountExistsWithTradings()
        {
            // Arrange
            string userId = "user1";

            // Act
            var result = _creditsRepository.GetAllTrading(userId);

            // Assert
            Assert.IsNotNull(result, "The returned list of tradings should not be null.");
            Assert.AreEqual(2, result.Count, "The number of tradings returned should be 1.");
            Assert.AreEqual("Bank", result.First().TdMethodPayment, "The method payment should be 'Credit Card'.");
        }

        [Test]
        public void TestGetAllTrading_AccountExistsWithoutTradings()
        {
            // Arrange
            string userId = "user3"; // User 2 không có giao dịch nào hợp lệ

            // Act
            var result = _creditsRepository.GetAllTrading(userId);

            // Assert
            Assert.IsNull(result, "The returned list of tradings should be null.");
        }

        [Test]
        public void TestGetAllTrading_AccountNotExists()
        {
            // Arrange
            string userId = "non_existing_user"; // Tài khoản không tồn tại

            // Act
            var result = _creditsRepository.GetAllTrading(userId);

            // Assert
            Assert.IsNull(result, "The returned value should be null when the account does not exist.");
        }

        [Test]
        public void TestGetTrading_WithoutTradings()
        {
            // Arrange
            string userId = "user2"; // User 2 có một giao dịch Payout

            // Act
            var result = _creditsRepository.GetAllTrading(userId);

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
