using Cursus.MVC.Models;

namespace Cursus.MVC.ViewModels
{
    public class CertificateViewModel
    {
        public CourseViewModel CourseVM { get; set; }
        public string FullName { get; set; }
        public EnrollViewModels EnrollVM { get; set; }
    }
}
