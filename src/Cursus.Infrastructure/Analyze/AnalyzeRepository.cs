using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Cursus.Application.Analyze;
using Cursus.Domain.Models;

namespace Cursus.Infrastructure.Analyze
{
    public class AnalyzeRepository : IAnalyzeRepository
    {
        private readonly CursusDBContext context;
        public AnalyzeRepository(CursusDBContext _context)
        {
            context = _context;
        }

        public List<Course> GetCourses()
        {
            return context.Courses.ToList();
        }

        public List<Domain.Models.Enroll> GetEnroll()
        {
            return context.Enrolls.ToList();
        }

        public MemoryStream ReportExcel(List<(int id, string coursename, int totalCourse, decimal totalMoney)> data)
        {
            var stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");
                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Course Name";
                worksheet.Cell(1, 3).Value = "Total Course";
                worksheet.Cell(1, 4).Value = "Total Money";
                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = data[i].id;
                    worksheet.Cell(i + 2, 2).Value = data[i].coursename;
                    worksheet.Cell(i + 2, 3).Value = data[i].totalCourse;
                    worksheet.Cell(i + 2, 4).Value = data[i].totalMoney;
                }
                workbook.SaveAs(stream);
            }
            stream.Position = 0;
            return stream;
        }
    }
}