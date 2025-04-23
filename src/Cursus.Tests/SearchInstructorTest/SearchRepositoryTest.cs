using Cursus.Domain.Models;
using Cursus.Infrastructure.SearchInstructor;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cursus.Tests.SearchInstructorTest
{
    public class SearchRepositoryTest
    {
        private SearchInstructorRepository _searchInstructorRepository;
        private CursusDBContext _context = null!;
        private Account _instructorAccount;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CursusDBContext(options);
            _searchInstructorRepository = new SearchInstructorRepository(_context);

            _instructorAccount = new Account
            {
                AccountId = 1,
                Username = "tandat",
                Email = "tandat123@gmail.com",
                FullName = "Duong Tan Dat",
                Role = 2, // instructor
                Password = "Abc123@",
                Bio = "Experienced instructor in C# and .NET",
            };

            // Add initial accounts to context
            _context.Accounts.Add(_instructorAccount);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetInstructorsByNameAsync_ReturnsCorrectInstructor_WhenNameMatches()
        {
            // Arrange
            string searchName = "tandat";

            // Act
            var result = await _searchInstructorRepository.GetInstructorsByNameAsync(searchName);

            // Assert
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsNotEmpty(result, "Result should not be empty.");
            Assert.AreEqual(1, result.Count(), "There should be one instructor returned.");
            Assert.AreEqual("tandat", result.First().Username, "The instructor's username should match.");
        }

        [Test]
        public async Task GetInstructorsByNameAsync_ReturnsEmpty_WhenNoInstructorMatches()
        {
            // Arrange
            string searchName = "non_existent";

            // Act
            var result = await _searchInstructorRepository.GetInstructorsByNameAsync(searchName);

            // Assert
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsEmpty(result, "Result should be empty as no instructor matches.");
        }

        [Test]
        public async Task GetInstructorsByNameAsync_ReturnsEmpty_WhenEmptyStringProvided()
        {
            // Arrange
            string searchName = string.Empty; // This should return all instructors with role 2

            // Act
            var result = await _searchInstructorRepository.GetInstructorsByNameAsync(searchName);

            // Assert
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.AreEqual(1, result.Count(), "There should be one instructor returned.");
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
