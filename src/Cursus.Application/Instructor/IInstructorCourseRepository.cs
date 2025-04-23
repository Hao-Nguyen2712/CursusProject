using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cursus.Application.Instructor
{
    public interface IInstructorCourseRepository
    {
        public List<Cursus.Domain.Models.Course> GetAllCourseByStatus(string status, int accountID);
    }
}