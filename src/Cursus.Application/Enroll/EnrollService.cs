
using Cursus.Application.Enroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.Application;
using Cursus.Domain.ViewModels;
using Cursus.Application.Account;


namespace Cursus.Application.Enroll
{
    public class EnrollService : IEnrollService
    {
        private readonly IEnrollRepository _enrollRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IAccountRepository _accountRepository;


        public EnrollService(IEnrollRepository enrollRepository, ICourseRepository courseRepository, IAccountRepository accountRepository)
        {
            _enrollRepository = enrollRepository;
        }


        public List<Domain.Models.Enroll> GetEnrollByAccountID(int accountID)
        {
            return _enrollRepository.GetEnrollByAccountID(accountID);
        }

        public Domain.Models.Enroll GetEnrollByAccountId(int id)
        {
            Domain.Models.Enroll enroll = _enrollRepository.GetEnrollByAccountId(id);
            return enroll;
        }

        public Domain.Models.Enroll GetEnrollByEnrollId(int id)
        {
            Domain.Models.Enroll enroll = _enrollRepository.GetEnrollByEnrollId(id);
            return enroll;
        }

        public Domain.Models.Enroll GetEnrollById(int id)
        {
            Domain.Models.Enroll enroll = _enrollRepository.GetEnrollById(id);
            return enroll;
        }

        public List<Domain.Models.Enroll> GetEnrolls()
        {
            var enrolls = _enrollRepository.GetEnrolls();
            return enrolls;
        }

        public List<Domain.Models.Enroll> GetAllEnroll()
        {
            // Call the underlying repository to retrieve all enrollments
            return _enrollRepository.GetAllEnroll();
        }

        public List<Domain.Models.Enroll> GetAllEnrollsByCourseId(int courseId)
        {
            return _enrollRepository.GetAllEnrollsByCourseId(courseId);
        }

        public List<Domain.Models.Enroll> GetAllEnrollsById(int accountId)
        {
            return _enrollRepository.GetAllEnrollsById(accountId);
        }

        public Domain.Models.Enroll GetAvailableEnroll(int courseId, int accountId)
        {
            // Call the underlying repository to retrieve the available enrollment
            return _enrollRepository.GetAvailableEnroll(courseId, accountId);
        }
        public Domain.Models.Enroll UpdateFinishByAccountID(int accountID, int courseID)
        {
            return _enrollRepository.UpdateFinishByAccountID(accountID, courseID);
        }
         public List<Domain.Models.Course> GetEnrolledCoursesByAccountId(int accountId)
        {
            return _enrollRepository.GetEnrolledCoursesByAccountId(accountId);
        }

        public void EnrollCourse(int courseId, int accountId)
        {
            _enrollRepository.EnrollCourse(courseId, accountId);
        }

        public void EnrollCourseFree(int courseId, int accountId)
        {
            _enrollRepository.EnrollCourseFree(courseId, accountId);
        }
        public Domain.Models.Enroll GetEnrollByAccoutIDAndCourseID(int accountID, int courseID)
        {
            return _enrollRepository.GetEnrollByAccoutIDAndCourseID(accountID, courseID);
        }
		public bool UnEnroll(int courseId, int accountId)
        {
            return _enrollRepository.UnEnroll(courseId, accountId);
        }

		public string GetEnrollStatus(int courseId, int accountId)
        {
            return _enrollRepository.GetEnrollStatus(courseId, accountId);
        }
		public bool DeleteProgressByCourseId(int courseId)
        {
            return _enrollRepository.DeleteProgressByCourseId(courseId);
        }

        public HomePageView GetData(int accountId, string id)
        {
            var enrolls = _enrollRepository.GetAllEnroll().Where(e => e.AccountId == accountId);
			var enrolledCourses = _enrollRepository.GetAllEnroll().Where(e => e.AccountId == accountId && e.EnrollStatus == "Enrolled");
            var enrollCourseIds = enrolls.Select(e => e.CourseId).ToList();
			var enrolledCourseIds = enrolledCourses.Select(e => e.CourseId).ToList();
            var enrollCourses = _courseRepository.GetAllCourse().Where(c => enrollCourseIds.Contains(c.CourseId )).ToList();
			var enrolledCourseList = _courseRepository.GetAllCourse().Where(c => enrolledCourseIds.Contains(c.CourseId )).ToList();
            var homePageView = new HomePageView()
            {
				EnrolledCourse = enrolledCourseList,
                EnrollCourse = enrollCourses
            };

            return homePageView;
        }
        public Domain.Models.Enroll getEnrollByStatus(int courseId, int accountID) {
            return _enrollRepository.getEnrollByStatus(courseId, accountID);
        }

        public List<Cursus.Domain.Models.Enroll> GetAllEnrolls()
        {
            return _enrollRepository.GetAllEnroll();
        }
	}
}
