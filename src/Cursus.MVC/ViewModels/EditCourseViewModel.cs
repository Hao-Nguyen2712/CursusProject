using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.MVC.Models;

namespace Cursus.MVC.ViewModels
{
    public class EditCourseViewModel
    {
        public List<LessonViewModel> LessonViewModels { get; set; }
        public CourseViewModel CourseViewModel { get; set; }
        public List<CategoryViewModel> CategoryViewModels { get; set; }
    }
}