using Cursus.Application.Category;
using Cursus.Application.Enroll;
using Cursus.Application;
using Cursus.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Cursus.Application.Account;
using AutoMapper;
using Cursus.MVC.Services;

using Microsoft.AspNetCore.Identity;
using Cursus.Infrastructure.Enroll;
using Cursus.MVC.Models;
using Cursus.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Cursus.Infrastructure;
using Cursus.Application.Certificate;
using Cursus.Application.Cart;

namespace Cursus.MVC.Controllers
{
    public class CertificateController : Controller
    {
        private readonly ISendEmail _sendEmail;
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollRepository _enrollRepository;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IAccountService _accountService;
        private readonly ICourseService _courseService;
        private readonly IEnrollService _enrollService;
        private readonly ICertificateService _certificateService;
        private readonly ICartService _cartService;

        public CertificateController(ISendEmail sendEmail, ICourseRepository courseRepository, IEnrollRepository enrollRepository, IMapper _mapper, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, IAccountService accountService, ICourseService courseService, IEnrollService enrollService, ICertificateService certificateService, ICartService cartService)
        {
            _sendEmail = sendEmail;
            this._courseRepository = courseRepository;
            this._enrollRepository = enrollRepository;
            this.mapper = _mapper;
            _userManager = userManager;
            _userStore = userStore;
            _accountService = accountService;
            _courseService = courseService;
            _enrollService = enrollService;
            _certificateService = certificateService;
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> CompleteEnroll(string userEmail, string subject)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("Email is required.");
            }

            // Giả sử bạn có logic để đánh dấu hoàn thành khóa học tại đây

            // Gửi email thông báo
            await _sendEmail.SendEmailAsync(userEmail, subject, 0);

            return Ok("Enroll completion email sent successfully.");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Lấy thông tin người dùng đã đăng nhập
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            // Lấy AccountID của người dùng
            var accountId = user.Id;
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int IdAccount = _accountService.GetAccountIDByUserID(userID);
            List<Enroll> listEnroll = _enrollService.GetEnrollByAccountID(IdAccount);

            List<EnrollViewModels> enroll = new List<EnrollViewModels>();

            foreach (var item in listEnroll)
            {
                if (item.EnrollStatus == "completed")
                {
                    EnrollViewModels enrollViewModels1 = new EnrollViewModels();
                    EnrollViewModel enrollView = mapper.Map<EnrollViewModel>(item);
                    Course course = _courseService.GetCourseById(item.CourseId.Value);
                    enrollViewModels1.CourseName = course.CourseName;
                    enrollViewModels1.Price = course.CourseMoney;
                    enrollViewModels1.EnrollViewModel = enrollView;
                    enroll.Add(enrollViewModels1);
                }
            }
            // Lọc các khóa học theo AccountID
            return View(enroll);
        }

        public IActionResult ViewCertificate(int id)
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userID))
            {
                return Unauthorized();
            }

            int IdAccount = _accountService.GetAccountIDByUserID(userID);
            string fullName = _accountService.getAccountName(userID);

            var certificate = _enrollService.GetEnrollByEnrollId(id);

            if (certificate == null)
            {
                return NotFound();
            }

            var certificateVM = mapper.Map<EnrollViewModel>(certificate);
            EnrollViewModels enrollViewModels = new EnrollViewModels();

            enrollViewModels.EnrollViewModel = certificateVM;
            enrollViewModels.FullName = fullName;

            var course = _courseService.GetCourseById(certificate.CourseId.Value);

            if (course == null)
            {
                return NotFound();
            }

            enrollViewModels.CourseName = course.CourseName;

            return View(enrollViewModels);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GenerateCertificate(int enrollId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int IdAccount = _accountService.GetAccountIDByUserID(userID);

            var Enroll = _enrollService.GetEnrollByEnrollId(enrollId);
            if (Enroll == null || Enroll.EnrollId != enrollId) // Giả sử enroll có thuộc tính AccountId
            {
                return NotFound();
            }

            var Account = _accountService.GetAccountByUserID(userID);
            if (Account == null || Account.Id != userID)
            {
                return Unauthorized();
            }

            //var enroll = _enrollService.GetEnrollByEnrollId(enrollId);
            var account = _accountService.GetAccountByUserID(userID);
            var courseID = _courseService.GetCourseIDByAccountID(IdAccount, enrollId);
            var courseName = _courseService.GetCourseNameByCourseID(courseID);
            if (Enroll == null)
            {
                return NotFound();
            }

            string fullName = account.FullName;
            string CourseName = courseName;
            string outputPath = $"{Enroll.EnrollId}.pdf";

            string downloadsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            // Tạo đường dẫn đầy đủ cho tệp PDF
            string outputPaths = Path.Combine(downloadsPath, outputPath);

            // Kiểm tra xem tệp đã tồn tại hay chưa
            if (System.IO.File.Exists(outputPaths))
            {
                return BadRequest("");
            }

            _certificateService.GenerateCertificateToPDF(fullName, CourseName, outputPath);

            return Ok(new { success = true });
        }
    }
}
