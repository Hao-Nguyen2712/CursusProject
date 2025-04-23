namespace Cursus.MVC.Models
{
    public class LessonViewModel
    {
        public int LessionId { get; set; }
        public int? CourseId { get; set; }
        public string LessionTilte { get; set; }
        public string LessionType { get; set; }
        public string LessionVideo { get; set; }
        public string LessionContent { get; set; }
        public int? LessionComments { get; set; }
        public int? LessionFinish { get; set; }
        public string LessionImage { get; set; }

        public virtual CourseViewModel CourseVM { get; set; }
        public virtual ICollection<CommentViewModel> CommentsVM { get; set; }
    }
}
