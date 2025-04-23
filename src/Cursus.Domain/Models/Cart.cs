using System;
using System.Collections.Generic;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Cart
    {
        public int CartId { get; set; }
        public int? CourseId { get; set; }
        public int? AccountId { get; set; }
        public decimal? CartMoney { get; set; }

        public virtual Account Account { get; set; }
        public virtual Course Course { get; set; }
    }
}
