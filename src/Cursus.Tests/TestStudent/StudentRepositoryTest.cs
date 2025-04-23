using Cursus.Domain.Models;
using Cursus.Infrastructure.Student;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Tests.TestStudent
{
    public class StudentRepositoryTest
    {
        private StudentRepository _studentRepository;
        private CursusDBContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CursusDBContext(options);
            _studentRepository = new StudentRepository(_context);


            var accounts = new List<Account>
            {
                new Account { AccountId = 1, Role = 3, IsDelete = "false" },
                new Account { AccountId = 2, Role = 2, IsDelete = "false" },
                new Account { AccountId = 3, Role = 3, IsDelete = "false" }
            };
            _context.Accounts.AddRange(accounts);
            _context.SaveChanges();
        }

        [Test]
        public void GetStudent()
        {
            // Act
            var students = _studentRepository.GetStudent();

            // Assert
            Assert.AreEqual(2, students.Count);
           
        }
        [Test]
        public void RemoveStudent()
        {
            // Act
            var result = _studentRepository.RemoveStudent(1);
            var students = _context.Accounts.FirstOrDefault(c => c.AccountId == 1);
            // Assert
            Assert.AreEqual("true", students.IsDelete);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
