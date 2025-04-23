using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.Domain.ViewModels;

namespace Cursus.Application.Instructor
{
    public interface IInstructorService
    {
        public List<Domain.Models.Account> GetInstructors();
        public bool RemoveInstructor(int id);
        public ProFileView GetInstructorProfile(string id);
    }
}