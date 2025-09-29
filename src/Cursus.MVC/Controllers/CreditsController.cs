using Microsoft.AspNetCore.Mvc;
using Cursus.Domain.Models;
using Cursus.Application.Credits;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Cursus.MVC.Models;
using Cursus.MVC.Services;
using Cursus.Application.Account;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using Cursus.MVC.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Cursus.MVC.Controllers
{
    public class CreditsController : Controller
    {
        private readonly ICreditsService _creditsService;
        private readonly IAccountService _accountService;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;


        public CreditsController(ICreditsService creditsService, IMapper mapper, IEmailSender emailSender, IAccountService accountService)
        {
            _creditsService = creditsService;
            _mapper = mapper;
            _emailSender = emailSender;
            _accountService = accountService;
        }

        [Authorize(Roles = "Student")]
        public IActionResult Index()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            double accMoney = _creditsService.GetAccMoney(userID);
            ViewBag.accMoney = accMoney;
            var list = _creditsService.GetAllTrading(userID);
            List<TradingViewModel> tradings = _mapper.Map<List<TradingViewModel>>(list);
            return View(tradings);
        }

        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            if (form == null)
            {
                return RedirectToAction("Error", "Home");

            }

            string balanceInput = form["balance[add]"];
            double total;

            if (!Double.TryParse(balanceInput, out total))
            {
                TempData["Status"] = "PayFail";
                return RedirectToAction("Error404", "Home");
            }

            if (total < 10)
            {
                TempData["Status"] = "PayFail";
                return RedirectToAction("Error404", "Home");
            }

            double money = _creditsService.ConvertUSDToVND(total).GetAwaiter().GetResult();
            int roundedInt = (int)Math.Round(money);

            var bankCode = form["card[expire-month]"].ToString();
            TempData["bankCode"] = bankCode;
            TempData["total"] = total.ToString();

            return Redirect(_creditsService.CreateRequestUrl(roundedInt, bankCode));
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Return()
        {
            var response = _creditsService.PaymentExcute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Status"] = "PayFail";
                return RedirectToAction("Error404", "Home");
            }

            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            string bankCode = TempData["bankCode"] as string;
            string totalStr = TempData["total"] as string;

            if (string.IsNullOrEmpty(totalStr) || string.IsNullOrEmpty(bankCode))
            {
                TempData["Status"] = "PayFail";
                return RedirectToAction("Error404", "Home");
            }

            int moneyacc;
            if(!Int32.TryParse(totalStr, out moneyacc))
            {
                TempData["Status"] = "PayFail";
                return RedirectToAction("Error404", "Home");
            }
            _creditsService.AddAccMoney(userID, moneyacc);

            var trading = new Domain.Models.Trading
            {
                TdDate = DateTime.Now,
                TdMoney = moneyacc,
                TdMethodPayment = bankCode
            };
            _creditsService.AddTrading(trading, userID);
            var account = _accountService.GetAccountByUserID(userID);
            var emailSender = (EmailSender)_emailSender;
            var htmlContent = emailSender.PaymentConfirm(account.FullName, moneyacc);
            await _emailSender.SendEmailAsync(account.Email, "Confirm Payment", htmlContent);
            TempData["Status"] = "PaySuccess";

            return RedirectToAction("Thanks", "Credits");
        }

        [Authorize(Roles = "Student")]
        public IActionResult Thanks()
        {
            return View();
        }
    }
}

