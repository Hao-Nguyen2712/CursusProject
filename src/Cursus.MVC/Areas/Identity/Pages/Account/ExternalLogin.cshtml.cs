// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Cursus.MVC.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Cursus.Domain.Models;
namespace Cursus.MVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<CursusMVCUser> _signInManager;
        private readonly UserManager<CursusMVCUser> _userManager;
        private readonly IUserStore<CursusMVCUser> _userStore;
        private readonly IUserEmailStore<CursusMVCUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly Cursus.Domain.Models.CursusDBContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ExternalLoginModel(
            SignInManager<CursusMVCUser> signInManager,
            UserManager<CursusMVCUser> userManager,
            IUserStore<CursusMVCUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            CursusDBContext db,
            IHttpContextAccessor httpContextAccessor,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Get the email from the external login info
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            // Check if the email already exists in the database
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                // Email exists, sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);

                // add session
                _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id);
                var userRole = await _userManager.GetRolesAsync(user);
                _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id);
                if (userRole.Contains("Admin"))
                {
                    returnUrl = Url.Content("~/Admin"); //  ~/
                }
                if (userRole.Contains("Instructor"))
                {
                    returnUrl = Url.Content("~/Instructor/DashBoard"); // chinh sua lai khi co trang Instructor
                }
                else if (userRole.Contains("Student"))
                {
                    returnUrl = Url.Content("~/Home");
                }
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            else
            {

                // Email does not exist, create a new user
                user = CreateUser();
                await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, email, CancellationToken.None);
                user.EmailConfirmed = true; // Mark email as confirmed

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        var userId = await _userManager.GetUserIdAsync(user);

                        // Create an Account entry in the database                        // Create an Account entry in the database
                        Cursus.Domain.Models.Account acc = new Cursus.Domain.Models.Account();
                        acc.Id = userId;
                        acc.Email = user.Email;
                        acc.Username = user.UserName;
                        acc.FullName = info.Principal.Identity.Name;
                        acc.Phone = "0123456789";
                        acc.Gender = "N/A";
                        acc.DateofBirth = DateTime.Now;
                        acc.Description = "Student";
                        acc.Role = 3;
                        acc.Avatar = "https://cursusstorageaccountv2.blob.core.windows.net/images/l60Hf-150x150.png";
                        acc.Money = 0;
                        acc.UpLevel = "False";
                        acc.IsDelete = "False";
                        acc.Bio = "N/A";
                        await _roleManager.CreateAsync(new IdentityRole("Student"));
                        await _userManager.AddToRoleAsync(user, "Student");
                        _db.Add(acc);
                        _db.SaveChanges();
                        // add session
                        _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id);

                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                ProviderDisplayName = info.ProviderDisplayName;
                ReturnUrl = returnUrl;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.EmailConfirmed = true; // Mark email as confirmed

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        var userId = await _userManager.GetUserIdAsync(user);

                        // Create an Account entry in the database
                        Cursus.Domain.Models.Account acc = new Cursus.Domain.Models.Account();
                        acc.Id = userId;
                        acc.Email = user.Email;
                        acc.Username = user.UserName;
                        acc.FullName = "N/A";
                        acc.Phone = "0123456789";
                        acc.Gender = "N/A";
                        acc.DateofBirth = DateTime.Now;
                        acc.Description = "Student";
                        acc.Role = 3;
                        acc.Avatar = "https://cursusstorageaccountv2.blob.core.windows.net/images/l60Hf-150x150.png";
                        acc.Money = 0;
                        acc.UpLevel = "False";
                        acc.IsDelete = "False";
                        acc.Bio = "N/A";
                        await _roleManager.CreateAsync(new IdentityRole("Student"));
                        await _userManager.AddToRoleAsync(user, "Student");
                        _db.Add(acc);
                        _db.SaveChanges();

                        // add session
                        _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id);

                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }

        private CursusMVCUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<CursusMVCUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(CursusMVCUser)}'. " +
                    $"Ensure that '{nameof(CursusMVCUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<CursusMVCUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<CursusMVCUser>)_userStore;
        }
    }
}
