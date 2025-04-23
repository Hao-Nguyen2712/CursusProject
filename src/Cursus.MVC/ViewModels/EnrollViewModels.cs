using Cursus.MVC.Models;

namespace Cursus.MVC.ViewModels
{
    public class EnrollViewModels
    {
        public string CourseName { get; set; }
        /*public string Vender {  get; set; }*/
        public decimal Price { get; set; }
        public EnrollViewModel EnrollViewModel { get; set; }
        public string FullName { get; set; }
        public CourseViewModel Course { get; set; }
    }
}
