using Microsoft.AspNetCore.Mvc;
using Cursus.MVC.Models;
using Cursus.Application;
using Cursus.Application.Category;
using Cursus.Application.DashBoard;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Cursus.Application;
using Cursus.Application.Enroll;
using Cursus.Application.Report;
using Cursus.Domain.Models;
using Cursus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Cursus.Application.Category;
using Cursus.Application.Instructor;
using Cursus.MVC.ViewModels;
using Cursus.Infrastructure.Category;
using Cursus.Application.SearchInstructor;
using Cursus.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Cursus.MVC.Areas.Identity.Data;
using System.Security.Claims;
using Cursus.Application.Account;
using Azure.Storage.Blobs;
using Cursus.Application.Cart;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Cursus.Application.Subscription;

using Cursus.Application.Profile;
using DocumentFormat.OpenXml.Drawing;


namespace Cursus.MVC.Controllers
{

    
    public class InstructorController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ILessonService _lessonService;
        private readonly ICategoryService _categoryService;
        private readonly IInstructorCourseService _instructorCourseService;
        private readonly IAccountService _accountService;

        private readonly IProfileService _profileService;
        private readonly IDashBoardService _dashBoardService;
        private readonly IMapper _mapper;
        private readonly ISearchInstructorService _searchInstructorService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly ICartService _cartService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IHomePageService _homePageService;
        private readonly IInstructorService _instructorService;

        // storage account connection string

        private string blobStorageConnectionString = "";
        private string blobStorageContainerName = "images";

        public InstructorController(ICourseService courseService, ILessonService lessonService, ICategoryService categoryService, IAccountService accountService, IInstructorCourseService instructorCourseService, IDashBoardService dashBoardSerVice, IMapper mapper, ISearchInstructorService searchInstructorService, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, ICartService cartService, ISubscriptionService subscriptionService, IProfileService ProfileService, IHomePageService homePageService, IInstructorService instructorService)
        {
            _courseService = courseService;
            _lessonService = lessonService;
            _categoryService = categoryService;
            _instructorCourseService = instructorCourseService;
            _accountService = accountService;
            _dashBoardService = dashBoardSerVice;
            _mapper = mapper;
            _searchInstructorService = searchInstructorService;
            _userManager = userManager;
            _userStore = userStore;
            _cartService = cartService;
            _subscriptionService = subscriptionService;
            _profileService = ProfileService;
            _homePageService = homePageService;
            _instructorService = instructorService;
        }


        [Authorize(Roles = "Instructor")]
        public IActionResult AddCourse()
        {
            var categories = _categoryService.GetAllCategories().Where(c => c.CategoryStatus == "Active").ToList();
            List<CategoryViewModel> addCourseViewModels = _mapper.Map<List<CategoryViewModel>>(categories);
            return View(addCourseViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> SearchInstructor(string name)
        {
            var instructors = await _searchInstructorService.SearchInstructorsAsync(name);
            List<SearchInstructorViewModel> instructorsVM = new List<SearchInstructorViewModel>();
            foreach (var instructor in instructors)
            {
                int countCourse = _courseService.GetCourseCountByAccountID(instructor.AccountId);
                int countSub = _courseService.GetSubByAccountID(instructor.Id);
                SearchInstructorViewModel searchInstructorViewModel = new SearchInstructorViewModel
                {
                    Instructor = _mapper.Map<AccountViewModel>(instructor),
                    CountSub = countSub,
                    CountCourse = countCourse,
                };
                instructorsVM.Add(searchInstructorViewModel);
            }

            ViewBag.name = name;
            return View(instructorsVM);
        }

        public async Task<IActionResult> ListSubscription(string instructorId)
        {

            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var account = _accountService.GetAccountIdById(userID);
            var accountId = account.AccountId;
            var homepage = _homePageService.GetData(accountId, userID);
            var homePageView = _mapper.Map<HomePageViewViewModel>(homepage);
            return View(homePageView);
        }

        [Authorize(Roles = "Instructor")]
        public IActionResult DashBoard()
        {
            // Get dashboard data from the service
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var account = _accountService.GetAccountIdById(userID);
            var accountId = account.AccountId;
            var dashboard = _dashBoardService.GetData(accountId, userID);
            var dashboardViewModel = _mapper.Map<DashBoardViewModel>(dashboard);

            // Return the view with the model
            return View(dashboardViewModel);
        }

        [HttpPost]
        public IActionResult AddCourse(AddCourseViewModels data)
        {
            var courseVM = data.course;
            var youtubeJson = data.youtube;

            courseVM.CourseDate = DateTime.Now;

            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int id = _accountService.GetAccountIDByUserID(userID);
            string thumbnail = HttpContext.Session.GetString("image");
            if (thumbnail != null)
            {
                courseVM.CourseAvatar = "https://cursusstorageaccountv2.blob.core.windows.net/images/" + thumbnail;
            }
            else
            {
                courseVM.CourseAvatar = "https://cursusstorageaccountv2.blob.core.windows.net/images/thumbnail-demo.jpg";
            }

            var course = _mapper.Map<Course>(courseVM);
            course.AccountId = id;
            Cursus.Domain.Models.Course courseModels = _courseService.AddCourse(course);
            if (courseModels == null)
            {
                return Json("false");
            }

            string LessonJsonList = HttpContext.Session.GetString("lessons");
            if (LessonJsonList != null)
            {
                int courseId = _courseService.GetCourseLastest();
                List<Lesson> lessons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Lesson>>(LessonJsonList);
                foreach (var lesson in lessons)
                {
                    lesson.CourseId = courseId;
                    lesson.LessionComments = 0;
                    lesson.LessionFinish = 0;
                    lesson.LessionType = "video";
                    lesson.LessionImage = "https://cursusstorageaccountv2.blob.core.windows.net/images/thumbnail-demo.jpg";
                    Lesson lesson1 = _lessonService.AddLesson(lesson);
                    if (lesson1 == null)
                    {
                        return Json("false");
                    }
                }
            }
            HttpContext.Session.Remove("lessons");
            HttpContext.Session.Remove("image");
            return Json("True");
        }

        public IActionResult AddLesson(string title, string description, string videoLink)
        {

            var lesson = new Domain.Models.Lesson
            {
                LessionTilte = title,
                LessionVideo = videoLink,
                LessionContent = description
            };

            string LessonJsonList = HttpContext.Session.GetString("lessons");
            int id = 0;

            if (LessonJsonList != null)
            {
                List<Lesson> lessonss = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Lesson>>(LessonJsonList);
                lessonss.Add(lesson);
                id = lessonss.Count;
                HttpContext.Session.SetString("lessons", Newtonsoft.Json.JsonConvert.SerializeObject(lessonss));
                return Json(new { title, description, videoLink, id = id });
            }

            List<Lesson> lessons = new List<Lesson>();
            lessons.Add(lesson);
            id = lessons.Count;
            HttpContext.Session.SetString("lessons", Newtonsoft.Json.JsonConvert.SerializeObject(lessons));
            return Json(new { title, description, videoLink, id = id });
        }

        [HttpPost]
        public IActionResult EditLesson(string title, string description, string videoLink, string id)
        {
            if (id == null || title == null || description == null || videoLink == null)
            {
                return Json("Error");
            }
            string LessonJsonList = HttpContext.Session.GetString("lessons");
            if (LessonJsonList != null)
            {
                List<Lesson> lessonss = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Lesson>>(LessonJsonList);
                // update lesson at 'id' in list lessons
                lessonss[(Int32.Parse(id) - 1)].LessionTilte = title;
                lessonss[(Int32.Parse(id) - 1)].LessionContent = description;
                lessonss[(Int32.Parse(id) - 1)].LessionVideo = videoLink;
                HttpContext.Session.SetString("lessons", Newtonsoft.Json.JsonConvert.SerializeObject(lessonss));
                return Json(new { title, description, videoLink, id });
            }
            return Json("Error");
        }

        public IActionResult RemoveLesson(string id)
        {
            if (id == null)
            {
                return BadRequest("Error");
            }
            string LessonJsonList = HttpContext.Session.GetString("lessons");
            if (LessonJsonList != null)
            {
                List<Lesson> lessonss = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Lesson>>(LessonJsonList);
                lessonss.RemoveAt(Int32.Parse(id) - 1);
                HttpContext.Session.SetString("lessons", Newtonsoft.Json.JsonConvert.SerializeObject(lessonss));
                return Json("True");
            }
            return BadRequest("Error");
        }


        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var validImageHeaders = new Dictionary<string, byte[]>
                {
                    { "image/jpeg", new byte[] { 0xFF, 0xD8, 0xFF } },
                    { "image/png", new byte[] { 0x89, 0x50, 0x4E, 0x47 } },
                    { "image/gif", new byte[] { 0x47, 0x49, 0x46, 0x38 } }
                };
                using (var stream = file.OpenReadStream())
                {
                    byte[] header = new byte[4];
                    await stream.ReadAsync(header, 0, header.Length);

                    bool isValid = validImageHeaders.Values.Any(h => header.Take(h.Length).SequenceEqual(h));
                    if (!isValid)
                    {
                        return Json("false");
                    }
                }
                var container = new BlobContainerClient(blobStorageConnectionString, blobStorageContainerName);
                var blob = container.GetBlobClient(file.FileName);

                if (await blob.ExistsAsync())
                {
                    // change the name of the file
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    blob = container.GetBlobClient(fileName);
                }

                using (var stream = file.OpenReadStream())
                {
                    await blob.UploadAsync(stream);
                }
                var url = blob.Uri.ToString();

                HttpContext.Session.SetString("image", file.FileName);
                return Json(new { Url = url });
            }
            return Json("false");
        }

        public IActionResult GetAllCourseByStatus(string status)
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int accountId = _accountService.GetAccountIDByUserID(userID);


            var myCourses = _mapper.Map<List<CourseViewModel>>(_instructorCourseService.GetAllCourseByStatus("Approved", accountId));
            var upcomingCourses = _mapper.Map<List<CourseViewModel>>(_instructorCourseService.GetAllCourseByStatus("Pending Approval", accountId));
            var rejectCourse = _mapper.Map<List<CourseViewModel>>(_instructorCourseService.GetAllCourseByStatus("Rejected", accountId));
            var blockCourse = _mapper.Map<List<CourseViewModel>>(_instructorCourseService.GetAllCourseByStatus("Blocked", accountId));
            var deleteCourse = _mapper.Map<List<CourseViewModel>>(_instructorCourseService.GetAllCourseByStatus("Deleted", accountId));

            var model = new Tuple<List<CourseViewModel>, List<CourseViewModel>, List<CourseViewModel>, List<CourseViewModel>, List<CourseViewModel>>(myCourses, upcomingCourses, rejectCourse, blockCourse, deleteCourse);

            if (status != null)
            {
                if (status == "Approved")
                {
                    return PartialView("MyCoursesPartial", model.Item1);
                }
                else if (status == "Pending Approval")
                {
                    return PartialView("UpcomingCoursesPartial", model.Item2);
                }
                if (status == "Rejected")
                {
                    return PartialView("RejectedCoursePartial", model.Item3);
                }
                else if (status == "Blocked")
                {
                    return PartialView("BlockedCoursePartial", model.Item4);
                }
                else if (status == "Deleted")
                {
                    return PartialView("DeletedCoursePartial", model.Item5);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ViewProfile(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (id == null)
            {
                ClaimsPrincipal claims = this.User;
                var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
                var account = _instructorService.GetInstructorProfile(userID);
                var accountViewModel = _mapper.Map<ProFileViewViewModel>(account);
                // send tempdata to view
                TempData["AccountId"] = "Yes";
                return View(accountViewModel);
            }
            else
            {
                var account = _instructorService.GetInstructorProfile(id);
                var accountViewModel = _mapper.Map<ProFileViewViewModel>(account);
                ViewBag.Yes = "ok";
                return View(accountViewModel);
            }
        }
        [HttpGet]
        public IActionResult EditAccount()
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var account = _accountService.GetAccountIdById(userID);
            var accountId = account.AccountId;
            var accountViewModel = _mapper.Map<AccountViewModel>(account);

            // Return the view with the model
            return View(accountViewModel);

        }
        [HttpPost]
        public IActionResult EditAccount(AccountViewModel accountViewModel)
        {

            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var account = _accountService.GetAccountIdById(userID);
            _mapper.Map(accountViewModel, account, typeof(AccountViewModel), typeof(Account));
            _accountService.EditAccount(account);
            return RedirectToAction("ViewProfile");

        }

        [HttpPost]
        public IActionResult ReSubmitCourse(IFormCollection iform)
        {
            int courseId = int.Parse(iform["courseId"]);
            Domain.Models.Course course = _courseService.UpdateCourseStatus(courseId);
            return RedirectToAction("GetAllCourseByStatus");
        }

        public IActionResult DeleteCourse(int id)
        {
            Course course = _courseService.DeleteCourse(id);
            if (course == null)
            {
                return Json("false");
            }
            return RedirectToAction("GetAllCourseByStatus");
        }

        [HttpGet]
        public IActionResult EditCourse(int id)
        {
            var course = _courseService.GetCourseById(id);

            if (course == null)
            {
                TempData["message"] = "Course not found";
                return RedirectToAction("GetAllCourseByStatus");
            }
            // get lesson by course id
            List<Lesson> lessons = _lessonService.GetLessonsByCourseID(course.CourseId);
            var categories = _categoryService.GetAllCategories();
            List<CategoryViewModel> addCourseViewModels = _mapper.Map<List<CategoryViewModel>>(categories);
            // map
            List<LessonViewModel> lessonViewModels = _mapper.Map<List<LessonViewModel>>(lessons);
            var courseViewModel = _mapper.Map<CourseViewModel>(course);

            EditCourseViewModel editCourseViewModel = new EditCourseViewModel
            {
                LessonViewModels = lessonViewModels,
                CourseViewModel = courseViewModel,
                CategoryViewModels = addCourseViewModels
            };

            // sesson save list lesson
            HttpContext.Session.SetString("lessons", Newtonsoft.Json.JsonConvert.SerializeObject(lessonViewModels));
            return View(editCourseViewModel);
        }

        public IActionResult EditCourse(AddCourseViewModels data)
        {
            var courseVM = data.course;
            var youtubeJson = data.youtube;
            string id = data.id;
            int courseId = int.Parse(id);
            if (courseVM.CourseAvatar == null)
            {
                courseVM.CourseAvatar = "https://cursusstorageaccountv2.blob.core.windows.net/images/thumbnail-demo.jpg";
            }
            else
            {
                courseVM.CourseAvatar = "https://cursusstorageaccountv2.blob.core.windows.net/images/" + courseVM.CourseAvatar;
            }
            Domain.Models.Course course = _courseService.GetCourseById(courseId);
            if (course == null)
            {
                TempData["message"] = "Course not found";
                return RedirectToAction("GetAllCourseByStatus");
            }
            Domain.Models.Course courseModels = _mapper.Map<Course>(courseVM);
            courseModels.CourseId = course.CourseId;
            Domain.Models.Course course1 = _courseService.UpdateCourseEdit(courseModels);
            if (course1 == null)
            {
                return Json("false");
            }
            // get list lesson from session
            string LessonJsonList = HttpContext.Session.GetString("lessons");
            if (LessonJsonList != null)
            {
                List<Lesson> lessonss = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Lesson>>(LessonJsonList);
                foreach (var lesson in lessonss)
                {
                    if (lesson.LessionId != 0)
                    {
                        _lessonService.UpadateLesson(lesson);
                    }
                    else
                    {
                        lesson.CourseId = course1.CourseId;
                        lesson.LessionComments = 0;
                        lesson.LessionFinish = 0;
                        lesson.LessionType = "video";
                        lesson.LessionImage = "https://cursusstorageaccountv2.blob.core.windows.net/images/thumbnail-demo.jpg";
                        Lesson lesson1 = _lessonService.AddLesson(lesson);
                        if (lesson1 == null)
                        {
                            return Json("false");
                        }
                    }
                }
            }
            // remove session
            HttpContext.Session.Remove("lessons");
            // TeamData["message"] = "Course updated successfully";
            return Json("True");
        }


        [HttpPost]
        public IActionResult GetLessonToEdit(int id)
        {
            // get list lesson from session
            string LessonJsonList = HttpContext.Session.GetString("lessons");
            if (LessonJsonList != null)
            {
                List<Lesson> lessonss = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Lesson>>(LessonJsonList);
                Lesson lesson = lessonss[id - 1];
                return Json(lesson);
            }
            return Json("False");
        }

        [HttpPost]
        public IActionResult UpdateLessonEdit()
        {
            // TODO: Your code here
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var container = new BlobContainerClient(blobStorageConnectionString, blobStorageContainerName);
                var blob = container.GetBlobClient(file.FileName);

                if (await blob.ExistsAsync())
                {
                    // change the name of the file
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    blob = container.GetBlobClient(fileName);
                }

                using (var stream = file.OpenReadStream())
                {
                    await blob.UploadAsync(stream);
                }
                var url = blob.Uri.ToString();

                ClaimsPrincipal claims = this.User;
                var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
                var account = _accountService.GetAccountIdById(userID);
                account.Avatar = url;
                var account2 = _accountService.UpdateAccount(account);
                TempData["Message"] = "AvatarSuccess";
                return RedirectToAction("ViewProfile", "Instructor");
            }
            TempData["Message"] = "AvatarFailed";
            return RedirectToAction("ViewProfile", "Instructor");
        }
        [HttpPost]
        public async Task<IActionResult> UploadAvatarStudent([FromForm] IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var container = new BlobContainerClient(blobStorageConnectionString, blobStorageContainerName);
                var blob = container.GetBlobClient(file.FileName);

                if (await blob.ExistsAsync())
                {
                    // change the name of the file
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    blob = container.GetBlobClient(fileName);
                }

                using (var stream = file.OpenReadStream())
                {
                    await blob.UploadAsync(stream);
                }
                var url = blob.Uri.ToString();

                ClaimsPrincipal claims = this.User;
                var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
                var account = _accountService.GetAccountIdById(userID);
                account.Avatar = url;
                var account2 = _accountService.UpdateAccount(account);
                TempData["Message"] = "AvatarSuccess";
                return RedirectToAction("ViewProfile", "Student");
            }
            TempData["Message"] = "AvatarFailed";
            return RedirectToAction("ViewProfile", "Student");
        }




    }
}