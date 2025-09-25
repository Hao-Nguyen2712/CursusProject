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
            throw new NotImplementedException();
        }

        public List<Domain.Models.Report> GetAllReport()
        {
            return _context.Reports.ToList();
        }

        public Domain.Models.Report GetReportById(int ReportId)
        {
            throw new NotImplementedException();
        }

        public void UpdateReport(Domain.Models.Report Report)
        {
            throw new NotImplementedException();
        }
    }
}