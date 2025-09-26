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
            _reportRepository.DeleteReport(ReportId);
        }

        public List<Domain.Models.Report> GetAllReport()
        {
            return _reportRepository.GetAllReport();
        }

        public Domain.Models.Report GetReportById(int ReportId)
        {
            return _reportRepository.GetReportById(ReportId);
        }

        public void UpdateReport(Domain.Models.Report Report)
        {
            _reportRepository.UpdateReport(Report);
        }

        public List<Domain.Models.Report> GetAllReports()
        {
            return _reportRepository.GetAllReport();
        }
    }
}