using Cursus.Application;
using Cursus.Application.Account;
using Cursus.Application.Cart;
using Cursus.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cursus.MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IAccountService _accountService;
        private readonly ICourseService _courseService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IEmailSender _sendEmail;


        public CartController(ICartService cartService, IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager, IAccountService accountService, ICourseService courseService, IEmailSender sendEmail)
        {
            _cartService = cartService;
            _userStore = userStore;
            _userManager = userManager;
            _accountService = accountService;
            _courseService = courseService;
            _sendEmail = sendEmail;
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddToCart(string courseID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { status = "NotAuthenticated" });
            }

            //get Id user from UserIdentity
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int id = _accountService.GetAccountIDByUserID(userID);
            int courseId = int.Parse(courseID);

            // check if the course is already purchased 
            if (_cartService.IsCoursePurchased(id, courseId))
            {
                return Json("Course already purchased.");
            }

            // check if the course is already in the cart
            var existingCartItem = _cartService.GetCartByAccountId(id).FirstOrDefault(c => c.Course.CourseId == courseId);
            if (existingCartItem != null)
            {
                return Json("Course already in cart.");
            }
            // Add course to cart
            Domain.Models.Course course = _courseService.GetCourseById(courseId);
            Domain.Models.Cart cart = new Domain.Models.Cart();
            cart.CourseId = courseId;
            cart.AccountId = id;
            cart.CartMoney = course.CourseMoney;
            if (cart != null)
            {
                _cartService.AddToCart(cart);
                var count = _cartService.GetCartByAccountId(id);
                var cartCount = count.Count;
                return Json(new { cartCount = cartCount });
            }

            return Json("True");
        }

        [HttpGet]
        public IActionResult GetCart(int accountId)
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int id = _accountService.GetAccountIDByUserID(userID);
            var cartItems = _cartService.GetCartByAccountId(id);

            ViewBag.CartCount = cartItems.Count;
            return View(cartItems);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int cartId)
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int id = _accountService.GetAccountIDByUserID(userID);

            var cartItem = _cartService.GetCartByAccountId(id).FirstOrDefault(c => c.CartId == cartId);
            if (cartItem != null)
            {
                _cartService.RemoveFromCart(cartId);
                var count = _cartService.GetCartByAccountId(id);
                var cartCount = count.Count;
                return Json(new { cartCount = cartCount });
            }

            return Json("Item not found.");
        }


        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int accountId = _accountService.GetAccountIDByUserID(userID);
            var account = _accountService.GetAccountByUserID(userID);

            bool checkoutResult = _cartService.Checkout(accountId);
            await _sendEmail.SendEmailAsync(account.Email, "Checkout", "Purchase confirmation email content");
            if (checkoutResult)
            {
                return RedirectToAction("Index", "Home"); // Redirect to success page
            }
            else
            {
                return RedirectToAction("Error", "Home"); // Redirect to error page
            }
        }

        [HttpPost]
        public async Task<IActionResult> BuyNow(string courseID)
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int accountId = _accountService.GetAccountIDByUserID(userID);
            int courseId = int.Parse(courseID);
            var account = _accountService.GetAccountByUserID(userID);

            // check if the course is already purchased
            if (_cartService.IsCoursePurchased(accountId, courseId))
            {
                return Json("Course already purchased.");
            }

            bool buyNow = _cartService.BuyNow(accountId, courseId);
            await _sendEmail.SendEmailAsync(account.Email, "Checkout", "Purchase confirmation email content");

            if (buyNow)
            {
                return RedirectToAction("Index", "Home"); // Redirect to success page
            }
            else
            {
                return RedirectToAction("Error", "Home"); // Redirect to error page
            }
        }
    }
}

