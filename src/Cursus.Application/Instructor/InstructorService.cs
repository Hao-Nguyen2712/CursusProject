using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.Application.Account;
using Cursus.Application.Enroll;
using Cursus.Application.Subscrise;
using Cursus.Domain.ViewModels;

namespace Cursus.Application.Instructor
{
    public class InstructorService : IInstructorService
    {
        private readonly IEnrollRepository _enrollRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ISubscriseRepository _subscriseRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorRepository _instructorRepository;
        public InstructorService(
            IEnrollRepository enrollRepository,
            IAccountRepository accountRepository,
            ICourseRepository courseRepository,
            ISubscriseRepository subscriseRepository,
        IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
            _enrollRepository = enrollRepository;
            _accountRepository = accountRepository;
            _courseRepository = courseRepository;
            _subscriseRepository = subscriseRepository;
        }

        public List<Domain.Models.Account> GetInstructors()
        {
            return _instructorRepository.GetInstructors();
        }

        public ProFileView GetInstructorProfile(string id)
        {
            var accounts = _accountRepository.GetAllAccount().ToList();
            var user = accounts.Where(a => a.Id == id).FirstOrDefault();
            var accountId = user.AccountId;
            var enrolls = _enrollRepository.GetAllEnroll().Where(e => e.AccountId == accountId);
            var courses = _courseRepository.GetAllCourse().Where(c => c.AccountId == accountId).ToList();
            var sub = _subscriseRepository.GetSubById(id).ToList();
            var subInStructorIds = sub.Select(s => s.InstructorId).ToList();
            var enrollCourseIds = enrolls.Select(e => e.CourseId).ToList();
            

            // Get courses that match the enrolled course IDs
            var subIntructor = _accountRepository.GetAllAccount().Where(a => subInStructorIds.Contains(a.Id)).ToList();
            var enrollCourses = _courseRepository.GetAllCourse().Where(c => enrollCourseIds.Contains(c.CourseId)).ToList();

            var proFileView = new ProFileView()
            {
                EnrollCourse = enrollCourses,
                CourseList = courses.Where(c => c.CourseStatus == "Approved").ToList(),
                SubscribeInstructor = subIntructor,                
                UserAcc = user
            };

            return proFileView;
        }

        public bool RemoveInstructor(int id)
        {
            return _instructorRepository.RemoveInstructor(id);
        }
    }
}