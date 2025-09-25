#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Account
    {
        public Account()
        {
            Carts = new HashSet<Cart>();
            Commnents = new HashSet<Comment>();
            Courses = new HashSet<Course>();
            Enrolls = new HashSet<Enroll>();
            Otps = new HashSet<Otp>();
            Rates = new HashSet<Rate>();
            Reports = new HashSet<Report>();
            Progresses = new HashSet<Progress>();
            UserSubscriptions = new HashSet<Subscribe>();
            InstructorSubscriptions = new HashSet<Subscribe>();
        }

        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public int? Role { get; set; }
        public decimal? Money { get; set; }
        public string UpLevel { get; set; }
        public string IsDelete { get; set; }
        public string Id { get; set; }
        public string Bio { get; set; }

        public virtual AspNetUser IdNavigation { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Comment> Commnents { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Enroll> Enrolls { get; set; }
        public virtual ICollection<Otp> Otps { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Progress> Progresses { get; set; }
        public virtual ICollection<Subscribe> UserSubscriptions { get; set; }
        public virtual ICollection<Subscribe> InstructorSubscriptions { get; set; }
    }
}
