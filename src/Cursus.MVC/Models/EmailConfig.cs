namespace Cursus.MVC.Models
{
    public class EmailConfig
    {
        public string SmtpHost { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 587;
        public string FromEmail { get; set; } = string.Empty;
        public string FromPassword { get; set; } = string.Empty;
        public bool EnableSsl { get; set; } = true;
        public string CompanyName { get; set; } = "Cursus";
        public string SupportEmail { get; set; } = "hao.nguyenchanh2712@gmail.com";
        public string CompanyAddress { get; set; } = "Thu Duc, Ho Chi Minh City, Viet Nam";
        public string CopyrightYear { get; set; } = DateTime.Now.Year.ToString();
    }
}