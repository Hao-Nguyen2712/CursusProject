// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Cursus.Domain;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Cursus.Domain.Models;

namespace Cursus.MVC.Areas.Identity.Pages.Account
{
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IUserStore<ApplicationUser> _userStore;
		private readonly IUserEmailStore<ApplicationUser> _emailStore;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IEmailSender _emailSender;
		private readonly Cursus.Domain.Models.CursusDBContext _db;
		private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            CursusDBContext db,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _db = db;
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
		public string ReturnUrl { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public IList<AuthenticationScheme> ExternalLogins { get; set; }

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
			[Display(Name = "Email")]
			public string Email { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }

			[Required]
            public string Role { get; set; }
        }


		public async Task OnGetAsync(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
			if (ModelState.IsValid)
			{
				var user = CreateUser();

				await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
				await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
				var result = await _userManager.CreateAsync(user, Input.Password);

				if (result.Succeeded)
				{
					/* await _roleManager.CreateAsync(new IdentityRole("Admin"));
					 await _userManager.AddToRoleAsync(user, "Admin");*/

					_logger.LogInformation("User created a new account with password.");

					var userId = await _userManager.GetUserIdAsync(user);
					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
					var callbackUrl = Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
						protocol: Request.Scheme);

					await _emailSender.SendEmailAsync(
						Input.Email, 
						"Confirm your email",
                        Service.EmailSender.EmailConfirm(user.UserName, $"<h3>To confirm your email address to register, please click the link below: </h3><div style='text-align: center;'><a style='color: white;' class='link-confirm' href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Confirm Email</a></div>"));

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
					{
						Cursus.Domain.Models.Account acc = new Cursus.Domain.Models.Account();
                        acc.Id = userId;
						acc.Email = Input.Email;
						acc.Password = Input.Password;
                        acc.FullName = "N/A";
                        acc.Phone = "0123456789";
                        acc.Gender = "N/A";
                        acc.DateofBirth = DateTime.Now;
						acc.Description = Input.Role;
                        if(acc.Description == "Instructor")
						{
                            acc.Description = "Student";
                            acc.Role = 3;
                            acc.UpLevel = "True";
                            await _roleManager.CreateAsync(new IdentityRole("Student"));
                            await _userManager.AddToRoleAsync(user, "Student");
                        } 
						else if(acc.Description == "Student")
						{
							acc.Role = 3;
                            acc.UpLevel = "False";
                            await _roleManager.CreateAsync(new IdentityRole("Student"));
                            await _userManager.AddToRoleAsync(user, "Student");
                        }
						acc.Avatar = "https://cursusstorageaccountv2.blob.core.windows.net/images/l60Hf-150x150.png";
                        acc.Money = 0;
                        acc.IsDelete = "False";
                        acc.Bio = "N/A";
                        acc.Username = user.UserName;
						await _db.AddAsync(acc);
						_db.SaveChanges();
						return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
					}
					else
					{
                        Cursus.Domain.Models.Account acc = new Cursus.Domain.Models.Account();
                        acc.Id = userId;
                        acc.Email = Input.Email;
                        acc.Password = Input.Password;
                        acc.FullName = "N/A";
                        acc.Phone = "0123456789";
                        acc.Gender = "N/A";
                        acc.DateofBirth = DateTime.Now;
                        acc.Description = Input.Role;
                        if (acc.Description == "Instructor")
                        {
                            acc.Description = "Student";
                            acc.Role = 3;
                            acc.UpLevel = "True";
                            await _roleManager.CreateAsync(new IdentityRole("Student"));
                            await _userManager.AddToRoleAsync(user, "Student");
                        }
                        else if (acc.Description == "Student")
                        {
                            acc.Role = 3;
                            acc.UpLevel = "False";
                            await _roleManager.CreateAsync(new IdentityRole("Student"));
                            await _userManager.AddToRoleAsync(user, "Student");
                        }
                        acc.Avatar = "https://cursusstorageaccountv2.blob.core.windows.net/images/l60Hf-150x150.png";
                        acc.Money = 0;
                        acc.IsDelete = "False";
                        acc.Bio = "N/A";
                        acc.Username = user.UserName;
                        await _db.AddAsync(acc);

						await _signInManager.SignInAsync(user, isPersistent: false);
						return LocalRedirect(returnUrl);
					}
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}

		private ApplicationUser CreateUser()
		{
			try
			{
				return Activator.CreateInstance<ApplicationUser>();
			}
			catch
			{
				throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
					$"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
					$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
			}
		}

		private IUserEmailStore<ApplicationUser> GetEmailStore()
		{
			if (!_userManager.SupportsUserEmail)
			{
				throw new NotSupportedException("The default UI requires a user store with email support.");
			}
			return (IUserEmailStore<ApplicationUser>)_userStore;
		}
	}
}
