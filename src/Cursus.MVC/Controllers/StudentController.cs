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
using System.Security.Claims;
using AutoMapper;
using Cursus.Domain.Models;
using Cursus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Cursus.Application.Account;
using Cursus.Application.Profile;
using Cursus.Application.Subscription;
using Cursus.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Cursus.Application.Cart;
namespace Cursus.MVC.Controllers
{

    public class StudentController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IProfileService _profileService;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        public StudentController(IAccountService accountService, IMapper mapper, IProfileService ProfileService, ISubscriptionRepository subscriptionRepository, ICartService cartService)
        {
            _mapper = mapper;
            _accountService = accountService;
            _profileService = ProfileService;
            _subscriptionRepository = subscriptionRepository;
            _cartService = cartService;
        }
        [HttpGet]
        [Authorize]
        public IActionResult ViewProfile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var account = _accountService.GetAccountIdById(userID);
            var accountID = account.AccountId;
            var course = _profileService.GetData(accountID, userID);

            // send session to view
            HttpContext.Session.SetString("AccountId", accountID.ToString());

            var accountViewModel = _mapper.Map<ProFileViewViewModel>(course);
            return View(accountViewModel);


        }
        [HttpGet]
        [Authorize]
        public IActionResult EditAccount()
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var account = _accountService.GetAccountIdById(userID);
            if (account == null)
            {
                // Log lỗi hoặc xử lý trường hợp không tìm thấy tài khoản
                return NotFound();
            }
            var accountViewModel = _mapper.Map<AccountViewModel>(account);
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


        [Authorize]
        public IActionResult UpLevelAccount()
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            Account account = _accountService.UpdateAccountLevel(userID);
            if (account == null)
            {
                TempData["Message"] = "Update level fail";
                return RedirectToAction("Index", "Home");
            }
            TempData["Message"] = "Update level success";
            return RedirectToAction("Index", "Home");
        }
    }
}