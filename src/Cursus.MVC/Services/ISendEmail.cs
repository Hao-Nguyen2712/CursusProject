namespace Cursus.MVC.Services
{
    public interface ISendEmail
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
