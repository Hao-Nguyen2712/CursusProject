using Cursus.Domain.Models;

namespace Cursus.MVC.Models
{
    public class OtpViewModel
    {
        public int OtpId { get; set; }
        public int? AccountId { get; set; }
        public string OtpType { get; set; }
        public DateTime? OtpCreateAt { get; set; }
        public DateTime? OtpExpiresAt { get; set; }
        public string OtpIsUse { get; set; }
        public int? OtpCode { get; set; }

        public virtual AccountViewModel AccountVM { get; set; }
    }
}
