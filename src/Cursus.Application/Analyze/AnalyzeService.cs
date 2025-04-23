using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application.Analyze
{
    public class AnalyzeService : IAnalyzeService
    {
        private readonly IAnalyzeRepository analyzeRepository;
        public AnalyzeService(IAnalyzeRepository _analyzeRepository)
        {
            analyzeRepository = _analyzeRepository;
        }



        public List<Course> GetCourses()
        {
            return analyzeRepository.GetCourses();
        }

        public List<Domain.Models.Enroll> GetEnroll()
        {
            return analyzeRepository.GetEnroll();
        }

        public MemoryStream ReportExcel(List<(int id, string coursename, int totalCourse, decimal totalMoney)> data)
        {
            return analyzeRepository.ReportExcel(data);
        }
    }
}