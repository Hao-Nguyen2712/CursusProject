using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cursus.Application.Analyze
{
    public interface IAnalyzeService
    {
        public List<Cursus.Domain.Models.Course> GetCourses();
        public List<Cursus.Domain.Models.Enroll> GetEnroll();
        public MemoryStream ReportExcel(List<(int id, string coursename, int totalCourse, decimal totalMoney)> data);
    }
}