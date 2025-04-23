using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.MVC.Models;
using Cursus.Domain.Models;

namespace Cursus.MVC.ViewModels
{
    public class InstructorViewModels
    {
        public string InstructorId { get; set; }
        public List<AccountViewModel> Accounts { get; set; }
        public List<Domain.Models.Subscribe> SubscribeVM { get; set; }
        public List<Domain.Models.AspNetUser> AspNetUserVM { get; set; }
    }
}