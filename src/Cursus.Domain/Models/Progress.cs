using System;
using System.Collections.Generic;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Progress
    {
        public int ProgressId { get; set; }
        public string AccId { get; set; }
        public int LessonId { get; set; }
        public string Finish { get; set; }
    }
}
