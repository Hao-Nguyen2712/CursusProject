using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.MVC.Models;

namespace Cursus.MVC.ViewModels
{
    public class AddCourseViewModels
    {
        public List<CategoryViewModel> Categories { get; set; }
        public CourseViewModel course { get; set; }
        public string youtube { get; set; }
        public string id { get; set; }
    }
}