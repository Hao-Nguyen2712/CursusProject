using System;
using System.Collections.Generic;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Trading
    {
        public int TdId { get; set; }
        public DateTime? TdDate { get; set; }
        public decimal? TdMoney { get; set; }
        public string TdMethodPayment { get; set; }
        public int? AccountId { get; set; }
    }
}
