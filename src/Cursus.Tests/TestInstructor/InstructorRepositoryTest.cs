using Cursus.Domain.Models;
using Cursus.Infrastructure.Instructor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Tests.TestInstructor
{
    public class InstructorRepositoryTest
    {
        private InstructorRepository _instructorRepository;
        private CursusDBContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CursusDBContext(options);
            _instructorRepository = new InstructorRepository(_context);


            var accounts = new List<Account>
            {
                new Account { AccountId = 1, Role = 2, IsDelete = "false" },
                new Account { AccountId = 2, Role = 1, IsDelete = "false" },
                new Account { AccountId = 3, Role = 2, IsDelete = "false" }
            };
            _context.Accounts.AddRange(accounts);
            _context.SaveChanges();
        }
        [Test]
        public void GetInstructors()
        {
            // Act
            var instructors = _instructorRepository.GetInstructors();

            // Assert
            Assert.AreEqual(2, instructors.Count);
          
        }
        [Test]
        public void RemoveInstructor_ShouldReturnFalse_WhenInstructorDoesNotExist()
        {
            // Act
            var result = _instructorRepository.RemoveInstructor(1);
            var instructor = _context.Accounts.FirstOrDefault(c => c.AccountId == 1);
            // Assert
            Assert.AreEqual("true", instructor.IsDelete);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
