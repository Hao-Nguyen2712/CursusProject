using Cursus.Application.Credits;
using Cursus.Application.Earning;
using Cursus.Application.Models;
using Cursus.Infrastructure.Earning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace Cursus.MVC.Controllers
{
	public class EarningController : Controller
	{
        private readonly IEarningService _earningService;
        private readonly ICreditsService _creditsService;

        public EarningController(IEarningService earningService, ICreditsService creditsService)
        {
            _earningService = earningService;
            _creditsService = creditsService;
        }

        [Authorize(Roles = "Instructor")]
        public IActionResult Index()
    {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

			double accMoney = _creditsService.GetAccMoney(userID);
            var totalInMonth = _earningService.GetTotalRevenueForCurrentMonth(userID);
            var totalInComeByDay = _earningService.TotalIncomebyDay(userID);
            var listTopCourse = _earningService.GetTop10Courses(userID);
            var listTopCourseEnroll = _earningService.GetTop10EnrollCounts(userID);

			ViewBag.AccMoney = accMoney;
            ViewBag.TotalInMonth = totalInMonth;
            ViewBag.TotalInComeByDay = totalInComeByDay;
			ViewBag.ListTopCourse = listTopCourse;
			ViewBag.ListTopCourseEnroll = listTopCourseEnroll;

			var topCourse = _earningService.GetTopSellingCourses(userID);
			return View(topCourse);
    }
        

	}
}
