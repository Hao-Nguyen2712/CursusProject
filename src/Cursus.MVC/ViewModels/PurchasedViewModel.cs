using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.MVC.Models;

namespace Cursus.MVC.ViewModels
{
    public class PurchasedViewModel
    {
        public CourseViewModel CourseVM { get; set; }
        public string isPurchase;
    }
}