using Cursus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Cursus.Application;
using Cursus.MVC.Areas.Identity.Data;
using Cursus.Application;
using AutoMapper;
using Cursus.Domain.Models;
using System.Security.Claims;
using Cursus.Domain.ViewModels;
using Cursus.Domain;
using Cursus.MVC.ViewModels;
using Cursus.Application.Account;
using Cursus.Application.Cart;
using Cursus.Application.Subscrise;
using Cursus.Application.Subscription;

namespace Cursus.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICartService _cartService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IHomePageService _homePageService;
        private readonly ISubscriptionService _subscriptionService;

        public HomeController(ICourseService courseService, IMapper mapper, IHomePageService homePageService, IAccountService accountService, ICartService cartService, ISubscriseService subscriseService, ISubscriptionService subscriptionService)
        {
            _courseService = courseService;
            _accountService = accountService;
            _cartService = cartService;
            _mapper = mapper;
            _homePageService = homePageService;
            _subscriptionService = subscriptionService;
        }

        public IActionResult Index()
        {

            if (!islogin())
            {
                var homepage1 = _homePageService.GetDataUnauthenticated();
                var homePageView1 = _mapper.Map<HomePageViewViewModel>(homepage1);
                return View(homePageView1);
            }


            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userID == null)
            {
                // Xử lý khi không tìm thấy userID
                return Redirect("~/Identity/Account/Login");
            }

            var account = _accountService.GetAccountIdById(userID);

            if (account == null)
            {
                // Xử lý khi không tìm thấy tài khoản
                return Redirect("~/Identity/Account/Login");
            }

            if(account.Role == 1){
                return RedirectToAction("Index", "Admin");
            }else if(account.Role == 2){
                return RedirectToAction("Dashboard", "Instructor");
            }
            
            // check user chua co thong tin
            if (string.IsNullOrEmpty(account.FullName) ||
                    string.IsNullOrEmpty(account.Phone) ||
                    string.IsNullOrEmpty(account.Gender))
            {
                // check role student
                if (account.Role == 3)
                {
                    return RedirectToAction("EditAccount", "Student");
                }
                else if (account.Role == 2)
                {
                    return RedirectToAction("EditAccount", "Instructor");
                }
            }
            var accountId = account.AccountId;
            var homepage = _homePageService.GetData(accountId, userID);
            var homePageView = _mapper.Map<HomePageViewViewModel>(homepage);

            // Return the view with the model
            return View(homePageView);
        }

        public IActionResult Error404()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public bool islogin()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return false;
                }
                ClaimsPrincipal claims = User;
                var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }
    }
}
