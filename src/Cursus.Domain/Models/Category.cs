using System;
using System.Collections.Generic;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Category
    {
        public Category()
        {
            Courses = new HashSet<Course>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryStatus { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
