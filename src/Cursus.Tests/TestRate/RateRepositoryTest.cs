using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Cursus.Tests.TestRate
{
    public class RateRepositoryTest
    {
        private RateRepository _rateRepository;
        private CursusDBContext _context { get; set; } = null!;
        private Rate _rate;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CursusDBContext(options);
            _rateRepository = new RateRepository(_context);

            _rate = new Rate
            {
                CourseId = 1,
                AccountId = 1,
                RatePoint = 5,
                RateContent = "RateContent",
                RateDate = DateTime.Now
            };
        }

        [Test]
        public void TestCreateRate_RateIsValid_RateIsAddedToDatabase()
        {
            // Act
            _rateRepository.CreateRate(_rate);
            _context.SaveChanges();

            // Assert
            Assert.IsNotNull(_context.Rates.FirstOrDefault(r => r.CourseId == _rate.CourseId && r.AccountId == _rate.AccountId), "CreateRate should add the rate to the database");
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