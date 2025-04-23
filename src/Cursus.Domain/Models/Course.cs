using System;
using System.Collections.Generic;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Course
    {
        public Course()
        {
            Carts = new HashSet<Cart>();
            Enrolls = new HashSet<Enroll>();
            Lessons = new HashSet<Lesson>();
            Rates = new HashSet<Rate>();
        }

        public int CourseId { get; set; }
        public int? AccountId { get; set; }
        public string CourseName { get; set; }
        public string CourseShortDes { get; set; }
        public string CourseDescription { get; set; }
        public string CourseWlearn { get; set; }
        public string CourseRequirement { get; set; }
        public string CourseAvatar { get; set; }
        public DateTime? CourseDate { get; set; }
        public decimal CourseMoney { get; set; }
        public string CourseStatus { get; set; }
        public int? CourseProcess { get; set; }
        public decimal? Discount { get; set; }
        public int? CategoryId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Enroll> Enrolls { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
    }
}
