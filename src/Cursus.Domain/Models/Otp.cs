#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Cursus.Domain.Models
{
    public partial class Otp
    {
        [Key]
        public int OtpId { get; set; }
        public int? AccountId { get; set; }
        public string OtpType { get; set; }
        public DateTime? OtpCreateAt { get; set; }
        public DateTime? OtpExpiresAt { get; set; }
        public string OtpIsUse { get; set; }
        public int? OtpCode { get; set; }

        public virtual Account Account { get; set; }
    }
}
