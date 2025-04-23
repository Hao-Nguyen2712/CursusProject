using Cursus.Domain.Models;

namespace Cursus.MVC.Models
{
    public class AccountViewModel
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Description { get; set; }
        public string? Avatar { get; set; }
        public int? Role { get; set; }
        public decimal? Money { get; set; }
        public string? UpLevel { get; set; }
        public string? IsDelete { get; set; }
        public string Id { get; set; }
        public string? Bio { get; set; }
        public virtual AspNetUser? IdNavigation { get; set; }
        public virtual ICollection<CartViewModel>? CartVN { get; set; }
        public virtual ICollection<CommentViewModel>? CommentVM { get; set; }
        public virtual ICollection<CourseViewModel>? CourseVM { get; set; }
        public virtual ICollection<EnrollViewModel>? EnrollVM { get; set; }
        public virtual ICollection<OtpViewModel>? OtpVM { get; set; }
        public virtual ICollection<RateViewModel>? RateVM { get; set; }
    }
}