using Cursus.Application.Report;
using Cursus.Domain.Models;

namespace Cursus.Infrastructure.Report
{


    public class ReportRepository : IReportRepository
    {
        private readonly CursusDBContext _context;

        public ReportRepository(CursusDBContext context)
        {
            _context = context;
        }

        public void CreateReport(Domain.Models.Report Report)
        {
            _context.Reports.Add(Report);
            _context.SaveChanges();
        }

        public void DeleteReport(int ReportId)
        {
            var report = _context.Reports.Find(ReportId);
            if (report != null)
            {
                _context.Reports.Remove(report);
                _context.SaveChanges();
            }
        }

        public List<Domain.Models.Report> GetAllReport()
        {
            return _context.Reports
                .OrderByDescending(r => r.ReportDate)
                .ToList();
        }

        public Domain.Models.Report GetReportById(int ReportId)
        {
            return _context.Reports.Find(ReportId);
        }

        public void UpdateReport(Domain.Models.Report Report)
        {
            _context.Reports.Update(Report);
            _context.SaveChanges();
        }
    }
}