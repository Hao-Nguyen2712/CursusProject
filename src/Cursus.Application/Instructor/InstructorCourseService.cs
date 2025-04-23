using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cursus.Application.Instructor
{
    public class InstructorCourseService : IInstructorCourseService
    {
        private readonly IInstructorCourseRepository _incourseRepository;
        public InstructorCourseService(IInstructorCourseRepository incourseRepository){
            _incourseRepository = incourseRepository;
        }
        public List<Domain.Models.Course> GetAllCourseByStatus(string status, int accountID)
        {
            var listCourseByStatus = _incourseRepository.GetAllCourseByStatus(status, accountID);
            return listCourseByStatus;
        }
    }
}