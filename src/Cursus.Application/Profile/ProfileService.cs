using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.Account;
using Cursus.Application.Enroll;
using Cursus.Application.Subscrise;
using Cursus.Domain.ViewModels;

namespace Cursus.Application.Profile
{
    
        public class ProfileService : IProfileService
    {
        private readonly IEnrollRepository _enrollRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ISubscriseRepository _subscriseRepository;


        private readonly ICourseRepository _courseRepository;
        public ProfileService(
            IEnrollRepository enrollRepository,

            IAccountRepository accountRepository,
            ICourseRepository courseRepository,
            ISubscriseRepository subscriseRepository)
        {
            _enrollRepository = enrollRepository;

            _accountRepository = accountRepository;
            _courseRepository = courseRepository;
            _subscriseRepository = subscriseRepository;
        }
        public ProFileView GetData(int accountId, string id)
        {
            //get all data
            var enrolls = _enrollRepository.GetAllEnroll().Where(e => e.AccountId == accountId && e.EnrollStatus == "Enrolled");
            var puchaseEnrolls = _enrollRepository.GetAllEnroll().Where(e => e.AccountId == accountId && e.EnrollStatus == "Purchased");
            var accounts = _accountRepository.GetAllAccount().ToList();
            var courses = _courseRepository.GetAllCourse().ToList();
            var sub = _subscriseRepository.GetSubById(id).ToList();
            var subInStructorIds = sub.Select(s => s.InstructorId).ToList();
            var enrollCourseIds = enrolls.Select(e => e.CourseId).ToList();
            var puchaseCourseIds = puchaseEnrolls.Select(e => e.CourseId).ToList();
            

            // Get courses that match the enrolled course IDs
            var subIntructor = _accountRepository.GetAllAccount().Where(a => subInStructorIds.Contains(a.Id)).ToList();
            var enrollCourses = _courseRepository.GetAllCourse().Where(c => enrollCourseIds.Contains(c.CourseId)).ToList();
            var puchaseCourses = _courseRepository.GetAllCourse().Where(c => puchaseCourseIds.Contains(c.CourseId)).ToList();

            var proFileView = new ProFileView()
            {
                EnrollCourse = enrollCourses,
                PurchasedCourse = puchaseCourses,
                CourseList = courses.Where(c => c.CourseStatus == "Approved").ToList(),
                SubscribeInstructor = subIntructor,                
                UserAcc = accounts.Where(a => a.AccountId == accountId).FirstOrDefault()
            };

            return proFileView;
        }

    }
}