using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.MVC.Models;

namespace Cursus.MVC.ViewModels
{
    public class SearchInstructorViewModel
    {
        public AccountViewModel Instructor { get; set; }
        public int CountCourse { get; set; }
        public int CountSub { get; set; }
    }
}