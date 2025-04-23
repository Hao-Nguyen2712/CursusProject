using System;
using System.Collections.Generic;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class Subscribe
    {
        public int SubId { get; set; }
        public string UserId { get; set; }
        public string InstructorId { get; set; }
    }
}
