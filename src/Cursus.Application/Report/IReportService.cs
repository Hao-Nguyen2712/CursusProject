using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cursus.Application.Report
{
    public interface IReportService
    {
                List<Cursus.Domain.Models.Report> GetAllReport();
        Cursus.Domain.Models.Report GetReportById(int ReportId);
        void CreateReport(Cursus.Domain.Models.Report Report);
        void UpdateReport(Cursus.Domain.Models.Report Report);
        void DeleteReport(int ReportId);
    }
}