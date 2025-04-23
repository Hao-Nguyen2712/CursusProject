using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application.Report
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        public ReportService (IReportRepository reportRepository){
            _reportRepository = reportRepository;
        }

        public void CreateReport(Domain.Models.Report Report)
        {
             _reportRepository.CreateReport(Report);
        }

        public void DeleteReport(int ReportId)
        {
            throw new NotImplementedException();
        }

        public List<Domain.Models.Report> GetAllReport()
        {
            return _reportRepository.GetAllReport();
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