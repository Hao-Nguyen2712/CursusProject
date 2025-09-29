using AutoMapper;
using Cursus.Application.Account;
using Cursus.Application.Credits;
using Cursus.Application.Payout;
using Cursus.MVC.Models;
using Cursus.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cursus.MVC.Controllers
{
    public class PayoutController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IEmailSender _emailSender;
        private readonly IPayoutService _payoutService;
        private readonly ICreditsService _creditsService;
        private readonly IMapper _mapper;

        public PayoutController(IPayoutService payoutService, IMapper mapper, IEmailSender emailSender, IAccountService accountService, ICreditsService creditsService)
        {
            _payoutService = payoutService;
            _mapper = mapper;
            _emailSender = emailSender;
            _accountService = accountService;
            _creditsService = creditsService;
        }

        [Authorize(Roles = "Instructor")]
        public IActionResult Index()
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userID))
            {
                TempData["Status"] = "InvalidUser";
                return RedirectToAction("Error404", "Home");
            }

            double accMoney = _creditsService.GetAccMoney(userID);
            ViewBag.accMoney = accMoney;
            TempData["accMoney"] = accMoney.ToString();

            var list = _payoutService.GetAllPayout(userID);
            List<TradingViewModel> tradings = _mapper.Map<List<TradingViewModel>>(list);

            return View(tradings);
        }

        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Index(IFormCollection form)
        {
            string? money = TempData["accMoney"] as string;
            if (string.IsNullOrEmpty(money) || !double.TryParse(money, out double accMoney))
            {
                TempData["Status"] = "InvalidAccountBalance";
                return RedirectToAction("Error404", "Home");
            }

            string? balanceInput = form["balance[add]"];
            if (string.IsNullOrEmpty(balanceInput))
            {
                TempData["Status"] = "MissingBalanceInput";
                return RedirectToAction("Error404", "Home");
            }

            if (!int.TryParse(balanceInput, out int total))
            {
                TempData["Status"] = "WithdrawnFail";
                return RedirectToAction("Error404", "Home");
            }

            if (total < 10)
            {
                TempData["Status"] = "WithdrawnFail";
                return RedirectToAction("Error404", "Home");
            }

            if (accMoney < total)
            {
                TempData["Status"] = "WithdrawnFail";
                return RedirectToAction("Error404", "Home");
            }

            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userID))
            {
                TempData["Status"] = "InvalidUser";
                return RedirectToAction("Error404", "Home");
            }
            // Update account money first
            var updatedAccount = _payoutService.UpdateAccMoney(userID, total);
            
            if (updatedAccount == null)
            {
                TempData["Status"] = "WithdrawnFail";
                return RedirectToAction("Error404", "Home");
            }

            var trading = new Domain.Models.Trading
            {
                TdDate = DateTime.Now,
                TdMoney = total
            };

            _payoutService.AddPayout(trading, userID);
            var account = _accountService.GetAccountByUserID(userID);

            if (account == null)
            {
                TempData["Status"] = "WithdrawnFail";
                return RedirectToAction("Error404", "Home");
            }
            
            // Send confirmation email
            var emailSender = (EmailSender)_emailSender;
            var htmlContent = emailSender.PayOutConfirm(account.FullName, total);
            await _emailSender.SendEmailAsync(account.Email, "Confirm Payment", htmlContent);
            
            TempData["Status"] = "WithdrawnSuccess";
            TempData["WithdrawnAmount"] = total;
            return RedirectToAction("Success", "Payout");
        }
        
        [Authorize(Roles = "Instructor")]
        public IActionResult Success()
        {
            // Pass the success message and withdrawn amount to the view
            ViewBag.SuccessMessage = TempData["Status"];
            ViewBag.WithdrawnAmount = TempData["WithdrawnAmount"];
            
            // Set a flag to redirect after 3 seconds
            ViewBag.RedirectToIndex = true;
            
            return View();
        }
    }
}

