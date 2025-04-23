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

namespace Cursus.Tests.AccountTest
{
    public class UpdateAccount
    {
        private AccountRepository _accountRepository;
        private CursusDBContext _context { get; set; } = null!;
        private Account _account;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CursusDBContext(options);
            _accountRepository = new AccountRepository(_context);

            _account = new Account
            {
                AccountId = 1,
                Username = "TestUser",
                Email = "test@example.com",
                Password = "password",
                FullName = "Test User",
                Phone = "1234567890",
                Gender = "Male",
                DateofBirth = DateTime.Now.AddYears(-25),
                Description = "Test Description",
                Avatar = "avatar.png",
                Role = 1,
                Money = 1000,
                UpLevel = "Level1",
                IsDelete = "False",
                Id = "unique-id",
                Bio = "Test Bio"
            };

            _context.Accounts.Add(_account);
            _context.SaveChanges();
        }

        [Test]
        public void TestUpdateAccount()
        {
            // Arrange
            _account.Username = "UpdatedUser";

            // Act
            var result = _accountRepository.UpdateAccount(_account);

            // Assert
            Assert.IsNotNull(result, "The account should be updated successfully.");
            Assert.AreEqual("UpdatedUser", result.Username, "The username should be updated.");
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