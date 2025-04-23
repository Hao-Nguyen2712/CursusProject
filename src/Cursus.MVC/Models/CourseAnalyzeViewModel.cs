using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cursus.MVC.Models
{
    public class CourseAnalyzeViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int TotalCourse { get; set; }
        public decimal TotalMoney { get; set; }
    }
}