using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cursus.Application.Subscription;
using Cursus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;
using Cursus.Application;
using Cursus.MVC.ViewModels;
using Cursus.Domain;
using Cursus.MVC.ViewModels;
using Cursus.Application.Account;
using Cursus.Application.Cart;
using System.Security.Claims;

namespace Cursus.MVC.ViewComponents
{
    public class SubscriptionViewComponent : ViewComponent
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IMapper _mapper;
        private readonly IHomePageService _homePageService;
        private readonly IAccountService _accountService;

        public SubscriptionViewComponent(ISubscriptionService subscriptionService, IMapper mapper, IHomePageService homePageService, IAccountService accountService)
        {
            _subscriptionService = subscriptionService;
            _mapper = mapper;
            _homePageService = homePageService;
            _accountService = accountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claims = HttpContext.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var account = _accountService.GetAccountIdById(userID);
            var accountId = account.AccountId;
            var homepage = _homePageService.GetData(accountId, userID);
            var homePageView = _mapper.Map<HomePageViewViewModel>(homepage);
            return View("SubscriptionList", homePageView);
        }
    }
}
// var limitedViewModels = homePageView.SubscribeInstructor.Take(2).ToList();