using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.Instructor;
using Cursus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Cursus.Infrastructure.Instructor
{
    public class InstructorCourseRepository : IInstructorCourseRepository
    {
        private readonly CursusDBContext _db;

        public InstructorCourseRepository(CursusDBContext dbContext)
        {
            _db = dbContext;
        }

        public List<Domain.Models.Course> GetAllCourseByStatus(string status, int accountID)
        {
            try
            {
                var courses = _db.Courses.Include(c => c.Category).Where(c => c.CourseStatus == status && c.AccountId == accountID).ToList();
                return courses;
            }
            catch (Exception ex)
            {
                return new List<Domain.Models.Course>();
            }
        }
    }
}