using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.Account;
using Cursus.Application.Cart;
using System.Security.Claims;
using Cursus.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Cursus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Cursus.Application;
using Cursus.Domain;


namespace Cursus.MVC.ViewComponents
{
    public class SetCartCountViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;
        private readonly IAccountService _accountService;

        public SetCartCountViewComponent(ICartService cartService, IAccountService accountService)
        {
            _cartService = cartService;
            _accountService = accountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal claims = HttpContext.User;
                var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
                int accountId = _accountService.GetAccountIDByUserID(userID);
                var cartItems = _cartService.GetCartByAccountId(accountId);
                ViewBag.CartCount = cartItems.Count;
                return View("setCartCount", ViewBag.CartCount);
            }
            else
            {
                ViewBag.CartCount = 0;
                return View("setCartCount", ViewBag.CartCount);
            }
        }
    }
}