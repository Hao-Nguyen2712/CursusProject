#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Cursus.Domain.Models
{
    public partial class Trading
    {
        [Key]
        public int TdId { get; set; }
        public DateTime? TdDate { get; set; }
        public decimal? TdMoney { get; set; }
        public string TdMethodPayment { get; set; }
        public int? AccountId { get; set; }
    }
}
