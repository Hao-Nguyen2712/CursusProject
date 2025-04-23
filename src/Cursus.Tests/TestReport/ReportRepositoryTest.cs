using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.Infrastructure.Report;
using Microsoft.EntityFrameworkCore;

namespace Cursus.Tests.TestReport
{
    public class ReportRepositoryTest
    {
        private ReportRepository _reportRepository;
        private CursusDBContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CursusDBContext(options);
            _reportRepository = new ReportRepository(_context);
        }

        [Test]
        public void TestCreateReport_ReportIsValid_ReportIsAddedToDatabase()
        {
            // Arrange
            var report = new Report
            {
                CourseId = 1,
                CmtId = 1,
                AccountId = 1,
                ReportReasson = "ReportReasson",
                ReportDate = DateTime.Now
            };

            // Act
            _reportRepository.CreateReport(report);
            _context.SaveChanges();

            // Assert
            Assert.IsNotNull(_context.Reports.FirstOrDefault(r => r.ReportId == report.ReportId), "CreateReport should add the report to the database");
        }

        [Test]
        public void TestCreateReport_ReportIsNull_ThrowsArgumentNullException()
        {
            // Act and Assert
            Assert.Throws<NullReferenceException>(() => _reportRepository.CreateReport(null));
        }

        [Test]
        public void TestGetAllReport_ReturnsAllReports()
        {
            // Arrange
            var report1 = new Report
            {
                CourseId = 1,
                CmtId = 1,
                AccountId = 1,
                ReportReasson = "ReportReasson1",
                ReportDate = DateTime.Now
            };
            var report2 = new Report
            {
                CourseId = 2,
                CmtId = 2,
                AccountId = 2,
                ReportReasson = "ReportReasson2",
                ReportDate = DateTime.Now
            };
            _context.Reports.Add(report1);
            _context.Reports.Add(report2);
            _context.SaveChanges();

            // Act
            var reports = _reportRepository.GetAllReport();

            // Assert
            Assert.AreEqual(2, reports.Count, "GetAllReport should return all reports");
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