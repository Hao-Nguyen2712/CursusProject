using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
