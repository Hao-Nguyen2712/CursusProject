using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.Account;
using Cursus.Application.Enroll;
using Cursus.Application.Subscrise;
using Cursus.Application;
using Cursus.Domain.ViewModels;
using Cursus.Application.Subscription;
using Cursus.Domain.Models;

namespace Cursus.Application
{
	public class HomePageService : IHomePageService
	{
		private readonly IEnrollRepository _enrollRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly ISubscriseRepository _subscriseRepository;
		private readonly ICourseRepository _courseRepository;
		private readonly ISubscriptionService _subscriptionService;
		private readonly ICourseService _courseService;
		private readonly IAccountService _accountService;
		private readonly IRateRepository _rateRepository;
		public HomePageService(
			IEnrollRepository enrollRepository,
			ISubscriseRepository subscriseRepository,
			IAccountRepository accountRepository,
			ICourseRepository courseRepository,
			ISubscriptionService subscriptionService,
			ICourseService courseService,
			IAccountService accountService,
			IRateRepository rateRepository)
		{
			_enrollRepository = enrollRepository;
			_subscriseRepository = subscriseRepository;
			_accountRepository = accountRepository;
			_courseRepository = courseRepository;
			_subscriptionService = subscriptionService;
			_courseService = courseService;
			_accountService = accountService;
			_rateRepository = rateRepository;
		}
		public HomePageView GetData(int accountId, string id)
		{
			//get all data
			var enrolls = _enrollRepository.GetAllEnroll().Where(e => e.AccountId == accountId);
			var enrolledCourses = _enrollRepository.GetAllEnroll().Where(e => e.AccountId == accountId && e.EnrollStatus == "Enrolled");
			var puchasedEnrolls = _enrollRepository.GetAllEnroll().Where(e => e.AccountId == accountId && e.EnrollStatus == "Purchased");
			var accounts = _accountRepository.GetAllAccount().ToList();
			var courses = _courseRepository.GetAllCourse().ToList();
			var sub = _subscriseRepository.GetSubById(id).ToList();
			var enrollCourseIds = enrolls.Select(e => e.CourseId).ToList();
			var puchasedEnrollCourseIds = puchasedEnrolls.Select(e => e.CourseId).ToList();
			var enrolledCourseIds = enrolledCourses.Select(e => e.CourseId).ToList();
			var subInStructorIds = sub.Select(s => s.InstructorId).ToList();

			// Get courses that match the enrolled course IDs
			var enrollCourses = _courseRepository.GetAllCourse().Where(c => enrollCourseIds.Contains(c.CourseId)).ToList();
			var puchasedEnrollCourses = _courseRepository.GetAllCourse().Where(c => puchasedEnrollCourseIds.Contains(c.CourseId)).ToList();
			var enrolledCourseList = _courseRepository.GetAllCourse().Where(c => enrolledCourseIds.Contains(c.CourseId)).ToList();
			var subIntructor = _accountRepository.GetAllAccount().Where(a => subInStructorIds.Contains(a.Id)).ToList();

			//get CourseRatings
			var allRates = _rateRepository.GetAllRate();
			var courseRatings = new Dictionary<int, double>();

			foreach (var course in courses)
			{
				var courseRates = allRates.Where(r => r.CourseId == course.CourseId);
				var averageRating = courseRates.Any() ? courseRates.Average(r => (double?)r.RatePoint) ?? 0.0 : 0.0;
				courseRatings.Add(course.CourseId, Math.Round(averageRating, 1));
			}
			var homePageView = new HomePageView()
			{
				EnrolledCourse = enrolledCourseList,
				PuchasedCourse = puchasedEnrollCourses,
				EnrollCourse = enrollCourses,
				CourseList = courses.Where(c => c.CourseStatus == "Approved").ToList(),
				InstructorsList = accounts.Where(a => a.Role == 2).ToList(),
				SubscribeInstructor = subIntructor,
				UserAcc = accounts.Where(a => a.AccountId == accountId).FirstOrDefault(),
				CourseRatings = courseRatings
			};

			return homePageView;
		}

		public HomePageView GetDataUnauthenticated()
		{
			List<Domain.Models.InstructorSubscription> listTopInstructor = _subscriptionService.GetTopSubscribedInstructors(7);
			List<Domain.Models.CourseTop> listTopCourse = _courseService.GetTopCourse(7);
			List<Course> listCourseVM = new List<Course>();
			List<Domain.Models.Account> listInstructorVM = new List<Domain.Models.Account>();
			foreach (var item in listTopCourse)
			{
				Domain.Models.Course course = _courseService.GetCourseById(item.CourseID);
				listCourseVM.Add(course);
			}
			foreach (var item in listTopInstructor)
			{
				Domain.Models.Account instructor = _accountService.GetAccountByUserID(item.InstructorId);
				listInstructorVM.Add(instructor);
			}


			var courses = _courseRepository.GetAllCourse().ToList();
			var accounts = _accountRepository.GetAllAccount().ToList();

			var allRates = _rateRepository.GetAllRate().ToList();

			var courseRatings = new Dictionary<int, double>();
			foreach (var course in courses)
			{
				var courseRates = allRates.Where(r => r.CourseId == course.CourseId);
				var averageRating = courseRates.Any() ? courseRates.Average(r => r.RatePoint) ?? 0.0 : 0.0;
				courseRatings.Add(course.CourseId, Math.Round(averageRating, 1));
			}

			var homePageView = new HomePageView()
			{
				CourseList = courses.Where(c => c.CourseStatus == "Approved").ToList(),
				InstructorsList = accounts.Where(a => a.Role == 2).ToList(),
				SubscribeInstructor = listInstructorVM,
				EnrollCourse = listCourseVM,
				EnrolledCourse = listCourseVM,
				PuchasedCourse = listCourseVM,
				UserAcc = null,
				CourseRatings = courseRatings
			};
			return homePageView;
		}
	}
}