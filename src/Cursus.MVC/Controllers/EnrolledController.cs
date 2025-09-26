using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.Enroll;
using Microsoft.AspNetCore.Mvc;
using Cursus.Application.Account;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using AutoMapper;
using Cursus.MVC.Models;
using Cursus.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Cursus.Application;
using Cursus.MVC.ViewModels;
using Cursus.Domain;
using Cursus.Application.Cart;

using Cursus.MVC.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace Cursus.MVC.Controllers
{
    public class EnrolledController : Controller
    {
       private readonly IEnrollService _enrollService;
        private readonly ICourseService _courseService;
        private readonly ICartService _cartService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IHomePageService _homePageService;
        private readonly IEmailSender _sendEmail;


        public EnrolledController(ICourseService courseService, IMapper mapper, IHomePageService homePageService, IEmailSender sendEmail, IAccountService accountService, ICartService cartService, IEnrollService enrollService)
        {
            _enrollService = enrollService;
            _courseService = courseService;
            _accountService = accountService;
            _cartService = cartService;
            _mapper = mapper;
            _homePageService = homePageService;
            _sendEmail = sendEmail;
            _courseService = courseService;
        }
       public IActionResult EnrolledCourses()
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var account = _accountService.GetAccountIdById(userID);
            var accountId = account.AccountId;
            var homepage = _homePageService.GetData(accountId, userID);
            var homePageView = _mapper.Map<HomePageViewViewModel>(homepage);
            return View(homePageView);
        }
        [HttpPost]
        public async Task<IActionResult> EnrollCourse(int courseId)
        {
            try
            {
                ClaimsPrincipal claims = this.User;
                var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
                int accountId = _accountService.GetAccountIDByUserID(userID);
                var account = _accountService.GetAccountByUserID(userID);
                var course = _courseService.GetCourseById(courseId);
                _enrollService.EnrollCourse(courseId, accountId);
                await _sendEmail.SendEmailAsync(account.Email, "Enroll Course", $"Enrolled in course: {course.CourseName}");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnrollCourseFree(int courseId)
        {
            try
            {
                ClaimsPrincipal claims = this.User;
                var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
                int accountId = _accountService.GetAccountIDByUserID(userID);
                _enrollService.EnrollCourseFree(courseId, accountId);
                var account = _accountService.GetAccountByUserID(userID);
                var course = _courseService.GetCourseById(courseId);
                _enrollService.EnrollCourse(courseId, accountId);
                await _sendEmail.SendEmailAsync(account.Email, "Enroll Course Free", $"Enrolled in free course: {course.CourseName}");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
