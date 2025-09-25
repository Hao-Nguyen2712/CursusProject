using System.ComponentModel.DataAnnotations;

namespace Cursus.Domain.Models
{
    public class InstructorSubscription
    {
        [Key]
        public string InstructorId { get; set; }
        public int SubscriptionCount { get; set; }

        // Navigation Property
        public virtual Account Instructor { get; set; }
    }
}
