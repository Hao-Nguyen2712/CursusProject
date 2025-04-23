using System;
using System.Collections.Generic;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            Commnents = new HashSet<Commnent>();
        }

        public int LessionId { get; set; }
        public int? CourseId { get; set; }
        public string LessionTilte { get; set; }
        public string LessionType { get; set; }
        public string LessionVideo { get; set; }
        public string LessionContent { get; set; }
        public int? LessionComments { get; set; }
        public int? LessionFinish { get; set; }
        public string LessionImage { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<Commnent> Commnents { get; set; }
    }
}
