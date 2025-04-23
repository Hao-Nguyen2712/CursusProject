using Cursus.Domain.Models;
using Cursus.MVC.Models;

namespace Cursus.MVC.ViewModels
{
	public class GetCourseViewModel
	{
		public CourseViewModel CourseVM { get; set; }
		public List<LessonViewModel> LessonVMs { get; set; }
		/*public List<CommentViewModel> CommentVMs { get; set; }*/
        public List<CommentAccoutViewModel> CommentAccoutViewModels { get; set; }
        public AccountViewModel AccountVM { get; set; }

		public LessonViewModel LessonVM { get; set; }
		public string Progress { get; set; }
        public List<string> progressList { get; set; }
    }
}
