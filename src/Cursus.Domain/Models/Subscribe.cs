#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Cursus.Domain.Models
{
    public partial class Subscribe
    {
        [Key]
        public int SubId { get; set; }
        
        public string UserId { get; set; }
        public string InstructorId { get; set; }

        // Navigation Properties
        public virtual Account User { get; set; }
        public virtual Account Instructor { get; set; }
    }
}
