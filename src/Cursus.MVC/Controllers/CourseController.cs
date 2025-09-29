using AutoMapper;
using Cursus.Application;
using Cursus.Application.Cart;
using Cursus.Application.Enroll;
using Cursus.Application.Progress;
using Cursus.Application.Report;
using Cursus.Application.Review;
using Cursus.Application.Subscrise;
using Cursus.Domain.Models;
using Cursus.MVC.Models;
using Cursus.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cursus.MVC.Controllers
{
    public class CourseController : Controller
    {
        private readonly Cursus.Application.ICourseService _courseService;
        private readonly Cursus.Application.ILessonService _lessonService;
        private readonly Cursus.Application.Category.ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly Cursus.Application.Account.IAccountService _accountService;
        private readonly Cursus.Application.Comment.ICommentService _commentService;
        private readonly IRateService _rateService;
        private readonly IEnrollService _enrollService;
        private readonly IReportService _reportService;
        private readonly ICartService _cartService;
        private readonly IProgressService _progressService;
        private readonly ISubscriseService _subscriseService;
        private readonly IEmailSender _sendEmail;
        private readonly IReviewService _reviewService;

        // constructor
        public CourseController
        (
            Cursus.Application.ICourseService courseService,
            Cursus.Application.ILessonService lessonService,
            Cursus.Application.Category.ICategoryService categoryService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            Cursus.Application.Account.IAccountService accountService,
            Application.Comment.ICommentService commentService,
            IRateService rateService,
            IEnrollService enrollService,
            IReportService reportService,
            ICartService cartService,
            IProgressService progressService,
            ISubscriseService subscriseService,
            IEmailSender sendEmail,
            IReviewService reviewService
        )
        {
            _courseService = courseService;
            _lessonService = lessonService;
            _categoryService = categoryService;
            _mapper = mapper;
            _userManager = userManager;
            _userStore = userStore;
            _accountService = accountService;
            _commentService = commentService;
            _rateService = rateService;
            _enrollService = enrollService;
            _reportService = reportService;
            _cartService = cartService;
            _progressService = progressService;
            _subscriseService = subscriseService;
            _sendEmail = sendEmail;
            _reviewService = reviewService;
        }

        public IActionResult Index()
        {
            if (_courseService == null)
            {
                throw new ArgumentNullException(nameof(_courseService), "Course service is not initialized.");
            }

            if (_mapper == null)
            {
                throw new ArgumentNullException(nameof(_mapper), "Mapper is not initialized.");
            }

            var listCourses = _courseService.GetAllCourse();
            if (listCourses == null)
            {
                return View("Error");
            }
            List<CourseViewModel> list = _mapper.Map<List<CourseViewModel>>(listCourses);
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> SearchCourse(string name)
        {
            var Course = await _courseService.GetCourseByNameAsync(name);
            List<CourseViewModel> courseVM = _mapper.Map<List<CourseViewModel>>(Course);
            ViewBag.name = name;
            return View(courseVM);
        }

        public IActionResult GetCourse(int courseID, int lessonID)

        {
            Domain.Models.Course course = _courseService.GetCourseById(courseID);
            List<Lesson> lessons = _lessonService.GetLessonsByCourseID(courseID);
            List<Cursus.Domain.Models.Comment> commnents = _commentService.GetCommentsByLessonID(lessonID);
            // map
            CourseViewModel courseVM = _mapper.Map<CourseViewModel>(course);
            List<LessonViewModel> lessonVMs = _mapper.Map<List<LessonViewModel>>(lessons);
            List<CommentViewModel> commentVMs = _mapper.Map<List<CommentViewModel>>(commnents);
            GetCourseViewModel getCourseViewModel = new GetCourseViewModel();
            List<CommentAccoutViewModel> ListCommentAcountVM = new List<CommentAccoutViewModel>();
            Lesson lesson = new Lesson();
            foreach (Lesson item in lessons)
            {
                if (item.LessionId == lessonID)
                {
                    lesson = item;
                }
            }
            if (lesson == null)
            {
                return RedirectToAction("/");
            }

            LessonViewModel lessonVM = _mapper.Map<LessonViewModel>(lesson);
            // loop comment
            foreach (CommentViewModel item in commentVMs)
            {
                // get account by lesson
                Domain.Models.Account account = _accountService.GetAccountByAccountID(item.AccountId ?? 0);
                ListCommentAcountVM.Add(new CommentAccoutViewModel
                {
                    AccountViewModel = _mapper.Map<AccountViewModel>(account),
                    CommentViewModel = item
                });
            }
            List<string> listProgress = new List<string>();
            getCourseViewModel.CourseVM = courseVM;
            getCourseViewModel.LessonVMs = lessonVMs;
            getCourseViewModel.CommentAccoutViewModels = ListCommentAcountVM;
            getCourseViewModel.LessonVM = lessonVM;
            getCourseViewModel.Progress = "False";
            // lessonVM
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal claims = this.User;
                var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
                Domain.Models.Account account = _accountService.GetAccountByUserID(userID);
                AccountViewModel accountVM = _mapper.Map<AccountViewModel>(account);
                getCourseViewModel.AccountVM = accountVM;

                // get get progress student study
                Domain.Models.Progress progress = _progressService.GetProgressByAccountIDAndLessonID(userID, lessonID);
                if (progress != null)
                {
                    getCourseViewModel.Progress = "True";
                }
                foreach (var item in lessons)
                {
                    Domain.Models.Progress progressItem = _progressService.GetProgressByAccountIDAndLessonID(userID, item.LessionId);
                    if (progressItem != null)
                    {
                        listProgress.Add("True");
                    }
                    else
                    {
                        listProgress.Add("False");
                    }
                }
                getCourseViewModel.progressList = listProgress;
            }
            return View(getCourseViewModel);
        }

        // [HttpGet("Course/{id}", Name = "CourseDetail")]
        public IActionResult GetCourseDetail(int id)
        {
            // if(!User.Identity.IsAuthenticated)
            // {
            //     return Unauthorized();
            // }
            var courseDetail = _courseService.GetCourseDetail(id);
            if (courseDetail == null)
            {
                return View("NotFound");
            }

            CourseViewModel detail = _mapper.Map<CourseViewModel>(courseDetail);

            if (detail == null)
            {
                return View("Error");
            }
            int lessonID = _lessonService.getLessonIDEsc(id);
            int enrollCount = _courseService.GetEnrolledCount(id);
            ViewBag.EnrollCount = enrollCount;

            // Check if the current user is enrolled in the course
            ViewBag.IsEnrolled = CheckEnrollStatus(id);
            var reviews = _reviewService.GetReviewByCourseId(id);
            ViewBag.Reviews = reviews;
            ViewBag.LessonID = lessonID;
            return View(detail);
        }

        [HttpGet]
        [Authorize]
        public bool CheckIsEnroll(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return false;
            }
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var account = _accountService.GetAccountIdById(userID);
            var accountId = account.AccountId;
            var enroll = _enrollService.GetAvailableEnroll(id, accountId);
            if (enroll == null)
            {
                return false;
            }
            var enrollViewModel = _mapper.Map<EnrollViewModel>(enroll);
            return enroll != null;
        }

        public string CheckEnrollStatus(int courseID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return "";
            }
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var account = _accountService.GetAccountIdById(userID);
            var accountId = account.AccountId;
            var enroll = _enrollService.getEnrollByStatus(courseID, accountId);
            if (enroll == null)
            {
                return "";
            }
            return enroll.EnrollStatus;
        }

        [HttpGet]
        public bool CheckIsSub(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return false;
            }
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var instructor = _accountService.GetAccountByAccountID(id);
            var instructorId = instructor.Id;
            var sub = _subscriseService.GetSubByInstructorAndUserId(instructorId, userID);
            if (sub == null)
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public IActionResult CheckEnroll(int id)
        {
            if (CheckIsEnroll(id))
            {
                return Json(new { enrolled = true });
            }
            else
            {
                return Json(new { enrolled = false });
            }
        }

        [HttpGet]
        public IActionResult CheckSub(int id)
        {
            if (CheckIsSub(id))
            {
                return Json(new { enrolled = true });
            }
            else
            {
                return Json(new { enrolled = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(int id)
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var instructor = _accountService.GetAccountByAccountID(id);
            var instructorMail = instructor.Email;
            var instructorUsername = instructor.Username;
            var instructorId = instructor.Id;
            var account = _accountService.GetAccountByUserID(userID);
            var subscrise = new SubscriseViewModel
            {
                InstructorId = instructorId,
                UserId = userID
            };
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }
            var subVM = _mapper.Map<Subscribe>(subscrise);
            _subscriseService.AddSub(subVM);
            await _sendEmail.SendEmailAsync(instructorMail, "Subscription Notification", $"New subscriber: {account.FullName}");
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UnSubscribe(int id)
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var instructor = _accountService.GetAccountByAccountID(id);
            var instructorMail = instructor.Email;
            var instructorId = instructor.Id;
            _subscriseService.UnSub(instructorId, userID);
            await _sendEmail.SendEmailAsync(instructorMail, "Subscription Notification", "User unsubscribed");
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult Rate(int id)
        {
            if (!CheckIsEnroll(id))
            {
                return RedirectToAction("GetCourseDetail", "Course", new { id });
            }

            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var course = _courseService.GetCourseById(id);
            var account = _accountService.GetAccountIdById(userID);
            var accountId = account.AccountId;
            var rate = new RateViewModel
            {
                CourseId = id,
                AccountId = accountId,
            };

            ViewBag.CourseName = course.CourseName;
            return View(rate);
        }

        [HttpPost]
        public IActionResult Rate(RateViewModel rateViewModel)
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var account = _accountService.GetAccountIdById(userID);
            var accountId = account.AccountId;
            if (!ModelState.IsValid)
            {
                return View(rateViewModel);
            }
            if (rateViewModel.RatePoint == null)
            {
                rateViewModel.RatePoint = 1;
            }
            rateViewModel.AccountId = accountId;
            var rate = _mapper.Map<Rate>(rateViewModel);
            _rateService.CreateRate(rate);

            return RedirectToAction("GetCourseDetail", "Course", new { id = rate.CourseId });
        }

        // [HttpGet]
        // public IActionResult Report(int id)
        // {
        //     if (!CheckIsEnroll(id))
        //     {
        //         return RedirectToAction("GetCourseDetail", "Course", new { id });
        //     }

        //     ClaimsPrincipal claims = this.User;
        //     var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
        //     var course = _courseService.GetCourseById(id);
        //     var account = _accountService.GetAccountIdById(userID);
        //     var accountId = account.AccountId;
        //     var report = new ReportViewModel
        //     {
        //         CourseId = id,
        //         AccountId = accountId,
        //         ReportDate = DateTime.Now
        //     };

        //     ViewBag.CourseName = course.CourseName;
        //     return View(report);
        // }

        [HttpPost]
        [Authorize]
        public IActionResult Report(ReportViewModel reportViewModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(reportViewModel);
            }

            var report = _mapper.Map<Report>(reportViewModel);
            _reportService.CreateReport(report);

            return RedirectToAction("GetCourseDetail", "Course", new { id = report.CourseId });
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UnEnroll(int courseId)
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int accId = _accountService.GetAccountIDByUserID(userID);
            if (accId == 0)
            {
                TempData["Status"] = "UnEnrollFail";
                return RedirectToAction("Error404", "Home");
            }

            var checkEnrollStatus = _enrollService.GetEnrollStatus(courseId, accId);


            if (checkEnrollStatus != "Enrolled")
            {
                TempData["Status"] = "UnEnrollFail";
                return RedirectToAction("Error404", "Home");
            }

            var result = _enrollService.UnEnroll(courseId, accId);

            if (!result)
            {
                TempData["Status"] = "UnEnrollFail";
                return RedirectToAction("Error404", "Home");
            }

            var deleteProgress = _enrollService.DeleteProgressByCourseId(courseId);

            if (!deleteProgress)
            {
                TempData["Status"] = "UnEnrollFail";
                return RedirectToAction("Error404", "Home");
            }

            var course = _courseService.GetCourseById(courseId);
            var account = _accountService.GetAccountByUserID(userID);
            await _sendEmail.SendEmailAsync(account.Email, "UnEnroll Course", $"Unenrolled from course: {course.CourseName}");
            TempData["Status"] = "UnEnrollSuccess";
            return RedirectToAction("GetCourseDetail", "Course", new { id = courseId });
        }

        [Authorize]
        public async Task<IActionResult> GetAllPurchasedCourse(int accountId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int id = await _accountService.GetAccountIDByUserIDAsync(userID);
            var course = _courseService.GetAllPurchasedCourseByAccountId(id);
            List<CourseViewModel> listCourse = _mapper.Map<List<CourseViewModel>>(course);
            List<PurchasedViewModel> listPurchase = new List<PurchasedViewModel>();
            foreach (var courseItem in listCourse)
            {
                Domain.Models.Enroll enroll = _enrollService.GetEnrollByAccoutIDAndCourseID(id, courseItem.CourseId);
                if (enroll == null)
                {
                    return RedirectToAction("/");
                }
                PurchasedViewModel pur = new PurchasedViewModel();
                pur.CourseVM = courseItem;
                pur.isPurchase = enroll.EnrollStatus;
                listPurchase.Add(pur);
            }
            return View(listPurchase);
        }
    }
}