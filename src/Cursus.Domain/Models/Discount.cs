using System;
using System.Collections.Generic;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Discount
    {
        public string DiscountId { get; set; }
        public string IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DiscountPercent { get; set; }
    }
}
