using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Progress
    {
        [Key]
        public int ProgressId { get; set; }
        
        public string AccId { get; set; }
        public int LessonId { get; set; }
        public string Finish { get; set; }

        // Navigation Properties
        public virtual Account Account { get; set; }
        public virtual Lesson Lesson { get; set; }
    }
}
