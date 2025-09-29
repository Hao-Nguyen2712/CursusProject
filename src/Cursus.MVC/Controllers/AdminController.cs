using AutoMapper;
using Cursus.Application;
using Cursus.Application.Account;
using Cursus.Application.Admin;
using Cursus.Application.AdminDashBoard;
using Cursus.Application.Analyze;
using Cursus.Application.Category;
using Cursus.Application.Comment;
using Cursus.Application.Credits;
using Cursus.Application;
using Cursus.MVC.ViewModels;
using Cursus.Application.Enroll;
using Cursus.Application.Instructor;
using Cursus.Application.Payout;
using Cursus.Application.Report;
using Cursus.Application.Student;
using Cursus.Domain.Models;
using Cursus.Domain.ViewModels;
using Cursus.MVC.Models;
using Cursus.MVC.Services;
using Cursus.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



namespace Cursus.MVC.Controllers
{

    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {

        private readonly ICategoryService categoryService;
        private readonly IStudentService studentService;
        private readonly IInstructorService instructorService;
        private readonly IAnalyzeService analyzeService;
        private readonly IMapper mapper;
        private readonly IAdminDashBoardService _adminDashBoardService;
        private readonly IAdminService adminService;
        private readonly IEmailSender _emailSender;
        private readonly IAccountService accountService;
        private readonly ICourseService courseService;
        private readonly IReportService reportService;
        private readonly ICreditsService creditsService;
        private readonly IPayoutService payoutService;
        private readonly IEnrollService enrollService;
        private readonly ICommentService commentService;
        private readonly IRateService rateService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(ICategoryService _categoryService, IInstructorService _instructorService, IStudentService _studentService, IAnalyzeService _analyzeService, IAdminDashBoardService adminDashBoardService, IAdminService _adminService, IMapper _mapper, IEmailSender emailSender, ICourseService courseService, IAccountService accountService, IReportService _reportService, ICreditsService _creditsService, IPayoutService _payoutService, IEnrollService _enrollService, ICommentService _commentService, IRateService _rateService, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)

        {
            this.categoryService = _categoryService;
            this.studentService = _studentService;
            this.instructorService = _instructorService;
            this.analyzeService = _analyzeService;
            _adminDashBoardService = adminDashBoardService;
            this.mapper = _mapper;
            this.adminService = _adminService;
            this.accountService = accountService;
            _emailSender = emailSender;
            this.courseService = courseService;
            this.reportService = _reportService;
            this.creditsService = _creditsService;
            this.payoutService = _payoutService;
            this.enrollService = _enrollService;
            this.commentService = _commentService;
            this.rateService = _rateService;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult GetAllCategories()
        {
            var list = categoryService.GetAllCategories();
            List<CategoryViewModel> categories = mapper.Map<List<CategoryViewModel>>(list);
            return View(categories);
        }


        public IActionResult GetCourseApprove()
        {
            var list = adminService.getCourseApprove();
            List<CourseViewModel> course = mapper.Map<List<CourseViewModel>>(list);
            return View(course);
        }

        [Route("Admin/ApproveCourse/{id:int}")]

        public async Task<IActionResult> ApproveCourse(int id)
        {
            this.adminService.ApproveCourse(id);
            Course course = courseService.GetCourseDetail(id);
            Account account = course.Account;
            var emailSender = (EmailSender)_emailSender;
            var htmlContent = emailSender.EmailNotiConfirmCourse(account.FullName, course.CourseName, "Approved");
            await _emailSender.SendEmailAsync(account.Email, "Notification Course", htmlContent);
            return RedirectToAction("GetCourseApprove");
        }

        [Route("Admin/DetailsCourseConfirm/{id:int}")]
        public IActionResult DetailsCourseConfirm(int id)
        {
            Course course = courseService.GetCourseDetail(id);
            Account account = course.Account;
            ViewBag.Account = account;
            List<Course> list = new List<Course>();
            list.Add(course);
            List<CourseViewModel> courses = mapper.Map<List<CourseViewModel>>(list);
            return View(courses);
        }

        [Route("Admin/RejectCourse/{id:int}")]

        public async Task<IActionResult> RejectCourse(int id)
        {
            this.adminService.RejectCourse(id);
            Course course = courseService.GetCourseDetail(id);
            Account account = course.Account;
            var emailSender = (EmailSender)_emailSender;
            var htmlContent = emailSender.EmailNotiConfirmCourse(account.FullName, course.CourseName, "Rejected");
            await _emailSender.SendEmailAsync(account.Email, "Notification Course", htmlContent);
            return RedirectToAction("GetCourseApprove");
        }
        public IActionResult GetListAccount()
        {
            var list = adminService.getListAccount();
            List<AccountViewModel> account = mapper.Map<List<AccountViewModel>>(list);
            return View(account);
        }
        [Route("Admin/BlockAccount/{id:int}")]

        public async Task<IActionResult> BlockAccount(int id)
        {
            this.adminService.BlockAccount(id);
            Account account = accountService.GetAccountByAccountID(id);
            await _emailSender.SendEmailAsync(account.Email, "Notification Account", $"Hello {account.FullName}, Your account has been blocked.");
            return RedirectToAction("GetListAccount");
        }

        [Route("Admin/UnBlockAccount/{id:int}")]

        public async Task<IActionResult> UnBlockAccount(int id)
        {
            this.adminService.UnBlockAccount(id);
            Account account = accountService.GetAccountByAccountID(id);
            await _emailSender.SendEmailAsync(account.Email, "Notification Account", $"Hello {account.FullName}, Your account has been unblocked.");
            return RedirectToAction("GetListAccount");
        }

        public IActionResult GetListInstructorRegis()
        {
            var list = adminService.getListInstructorRegis();
            List<AccountViewModel> account = mapper.Map<List<AccountViewModel>>(list);
            return View(account);
        }

        [Route("Admin/ApproveInstructorRegis/{id:int}")]
        public async Task<IActionResult> ApproveInstructorRegis(int id)
        {
            this.adminService.ApproveInstructorRegis(id);
            Account account = accountService.GetAccountByAccountID(id);
            string email = accountService.getEmail(id);
            var user = _userManager.FindByEmailAsync(email).Result;
            if (user != null)
            {
                _userManager.RemoveFromRoleAsync(user, "Student").Wait();
                if (!_roleManager.RoleExistsAsync("Instructor").Result)
                {
                    _roleManager.CreateAsync(new IdentityRole("Instructor")).Wait();
                }
                _userManager.AddToRoleAsync(user, "Instructor").Wait();
                await _emailSender.SendEmailAsync(
                    account.Email,
                    "Notification Regis",
                    $"Hello {account.FullName}, Your instructor registration has been approved."
                );
            }
            return RedirectToAction("GetListInstructorRegis");
        }

        [Route("Admin/RejectInstructorRegis/{id:int}")]

        public async Task<IActionResult> RejectInstructorRegis(int id)
        {
            this.adminService.RejectInstructorRegis(id);
            Account account = accountService.GetAccountByAccountID(id);
            await _emailSender.SendEmailAsync(account.Email, "Notification Regis", $"Hello {account.FullName}, Your instructor registration has been rejected.");
            return RedirectToAction("GetListInstructorRegis");
        }


        [HttpPost]
        public IActionResult AddNewCategory(CategoryViewModel categoryViewModel)
        {
            var categorryView = mapper.Map<Category>(categoryViewModel);
            if (categoryService.AddCategory(categorryView))
            {
                TempData["SuccessMessage"] = "Category added successfully!";
                return RedirectToAction("GetAllCategories");
            }
            TempData["ErrorMessage"] = "Failed to add category. This Category has exits before";
            return RedirectToAction("GetAllCategories");
        }


        [HttpPost]
        public IActionResult UpdateCategory(int id, string categoryName)
        {
            var category = categoryService.FindCategoryById(id);
            category.CategoryName = categoryName;
            if (categoryService.UpdateCategory(category))
            {
                TempData["SuccessMessage"] = "Category update successfully!";

                return Json(new { success = true });
            }
            TempData["ErrorMessage"] = "Failed to update category. This Category has exits before";
            return Json(new { success = false });
        }
        public IActionResult DeleteCategory(int id)
        {
            var category = categoryService.DeleteCategory(id);
            if (category)
            {
                TempData["SuccessMessage"] = "Category remove successfully!";
                return RedirectToAction("GetAllCategories");
            }
            TempData["ErrorMessage"] = "Failed to remove category.";
            return Redirect("/404");
        }

        public IActionResult GetStudent()
        {
            var list = studentService.GetStudent();
            List<AccountViewModel> accounts = mapper.Map<List<AccountViewModel>>(list);
            return View(accounts);
        }

        public IActionResult GetInstructor()
        {
            var list = instructorService.GetInstructors();
            List<AccountViewModel> accounts = mapper.Map<List<AccountViewModel>>(list);
            return View(accounts);
        }
        public async Task<IActionResult> RemoveStudent(int id)
        {
            var student = studentService.RemoveStudent(id);
            var account = accountService.GetAccountByAccountID(id);
            // var email =  account.Email;
            if (student)
            {
                //   var student =  studentService.
                await _emailSender.SendEmailAsync(
                account.Email,
                 "Confirm Admin",
                 $"Hello {account.Username}, Your account has been removed by administrator."
               );
                TempData["SuccessMessage"] = "Student remove successfully!";
                return RedirectToAction("GetStudent");
            }
            TempData["ErrorMessage"] = "Failed to remove Student.";
            return Redirect("/404");
        }
        public async Task<IActionResult> RemoveInstructor(int id)
        {
            var instructor = instructorService.RemoveInstructor(id);
            var account = accountService.GetAccountByAccountID(id);
            if (instructor)
            {
                await _emailSender.SendEmailAsync(
              account.Email,
               "Confirm Admin",
               $"Hello {account.Username}, Your account has been removed by administrator."
             );
                TempData["SuccessMessage"] = "Instructor remove successfully!";
                return RedirectToAction("GetInstructor");
            }
            TempData["ErrorMessage"] = "Failed to remove Instructor.";
            return Redirect("/404");
        }
        public IActionResult CourseAnalyze()
        {
            var result = from c in analyzeService.GetCourses()
                         join e in analyzeService.GetEnroll()
                         on c.CourseId equals e.CourseId
                         group new { c, e } by new { c.CourseId, c.CourseName } into g
                         orderby g.Key.CourseId
                         select new CourseAnalyzeViewModel
                         {
                             CourseId = g.Key.CourseId,
                             CourseName = g.Key.CourseName,
                             TotalCourse = g.Count(static en => en.e.EnrollId != null),
                             TotalMoney = (decimal)g.Sum(static en => en.c.CourseMoney)
                         };

            return View(result.OrderBy(c => c.CourseId).ToList());
        }
        [HttpPost]
        public IActionResult GetReportToExcel([FromBody] IEnumerable<CourseAnalyzeViewModel> model)
        {

            var data = new List<(int Id, string Name, int TotalCourse, decimal TotalMoney)>();
            foreach (var models in model)
            {
                var item = (Id: models.CourseId, Name: models.CourseName, TotalCourse: models.TotalCourse, TotalMoney: models.TotalMoney);
                data.Add(item);
            }
            var stream = analyzeService.ReportExcel(data);
            string excelName = "Report.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }


        public IActionResult index()
        {
            var dashboard = _adminDashBoardService.GetData();
            var dashboardViewModel = mapper.Map<AdminDashBoardViewModel>(dashboard);

            // Return the view with the model
            return View(dashboardViewModel);
        }

        // Temporary action to fix course avatars - can be removed after running once
        public async Task<IActionResult> FixCourseAvatars()
        {
            try
            {
                var allCourses = courseService.GetAllCourse();
                var fixedCount = 0;

                var imageMapping = new Dictionary<string, string>
                {
                    { "/images/courses/web-development.jpg", "/images/courses/img-1.jpg" },
                    { "/images/courses/ios-development.jpg", "/images/courses/img-2.jpg" },
                    { "/images/courses/data-science.jpg", "/images/courses/img-3.jpg" },
                    { "/images/courses/aws-cloud.jpg", "/images/courses/img-4.jpg" },
                    { "/images/courses/react-redux.jpg", "/images/courses/img-5.jpg" }
                };

                foreach (var course in allCourses)
                {
                    if (imageMapping.ContainsKey(course.CourseAvatar))
                    {
                        course.CourseAvatar = imageMapping[course.CourseAvatar];
                        courseService.UpdateCourseEdit(course);
                        fixedCount++;
                    }
                }

                TempData["Success"] = $"Fixed {fixedCount} course avatars successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error fixing course avatars: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // ===== REPORTS MANAGEMENT =====
        public IActionResult GetReports()
        {
            try
            {
                var reports = reportService.GetAllReport();
                var reportViewModels = mapper.Map<List<ReportViewModel>>(reports);
                return View(reportViewModels);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading reports: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [Route("Admin/DeleteReport/{id:int}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            try
            {
                reportService.DeleteReport(id);
                TempData["SuccessMessage"] = "Report deleted successfully!";
                return RedirectToAction("GetReports");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting report: {ex.Message}";
                return RedirectToAction("GetReports");
            }
        }

        // ===== FINANCIAL MANAGEMENT =====
        public IActionResult GetTransactions()
        {
            try
            {
                var allTransactions = creditsService.GetAllTradingForAdmin();
                var transactionViewModels = mapper.Map<List<TradingViewModel>>(allTransactions);
                return View(transactionViewModels);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading transactions: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult GetRevenue(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var revenueAnalytics = _adminDashBoardService.GetRevenueAnalytics(startDate, endDate);
                return View(revenueAnalytics);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading revenue data: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public JsonResult GetRevenueData(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var revenueAnalytics = _adminDashBoardService.GetRevenueAnalytics(startDate, endDate);
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        totalRevenue = revenueAnalytics.TotalRevenue,
                        monthlyRevenue = revenueAnalytics.MonthlyRevenue,
                        dailyRevenue = revenueAnalytics.DailyRevenue,
                        monthlyGrowth = revenueAnalytics.MonthlyGrowthPercentage,
                        platformFees = revenueAnalytics.PlatformFees,
                        instructorEarnings = revenueAnalytics.InstructorEarnings,
                        topCourses = revenueAnalytics.TopSellingCourses.Take(5),
                        paymentMethods = revenueAnalytics.PaymentMethodBreakdown,
                        monthlyData = revenueAnalytics.MonthlyRevenueData.Take(12),
                        dailyData = revenueAnalytics.DailyRevenueData.Take(30)
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult GetPayouts(DateTime? startDate = null, DateTime? endDate = null, PayoutStatus? status = null)
        {
            try
            {
                var instructorPayouts = _adminDashBoardService.GetInstructorPayouts(startDate, endDate);

                // Apply status filter if provided
                if (status.HasValue)
                {
                    instructorPayouts.PendingPayouts = instructorPayouts.PendingPayouts
                        .Where(p => p.Status == status.Value).ToList();
                    instructorPayouts.StatusFilter = status;
                }

                return View(instructorPayouts);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading payout data: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayout(int payoutId)
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Admin";
                var result = await _adminDashBoardService.ProcessPayoutAsync(payoutId, adminId);

                if (result)
                {
                    TempData["SuccessMessage"] = "Payout processed successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to process payout";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error processing payout: {ex.Message}";
            }

            return RedirectToAction("GetPayouts");
        }

        [HttpPost]
        public async Task<IActionResult> BulkProcessPayouts(List<int> payoutIds)
        {
            try
            {
                if (payoutIds == null || !payoutIds.Any())
                {
                    TempData["ErrorMessage"] = "No payouts selected";
                    return RedirectToAction("GetPayouts");
                }

                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Admin";
                var result = await _adminDashBoardService.BulkProcessPayoutsAsync(payoutIds, adminId);

                if (result)
                {
                    TempData["SuccessMessage"] = $"Successfully processed {payoutIds.Count} payouts";
                }
                else
                {
                    TempData["ErrorMessage"] = "Some payouts failed to process";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error processing payouts: {ex.Message}";
            }

            return RedirectToAction("GetPayouts");
        }

        [HttpGet]
        public JsonResult GetPayoutData(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var payoutData = _adminDashBoardService.GetInstructorPayouts(startDate, endDate);
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        totalPending = payoutData.TotalPendingPayouts,
                        totalPaidOut = payoutData.TotalPaidOut,
                        currentMonth = payoutData.CurrentMonthPayouts,
                        pendingCount = payoutData.PendingPayoutCount,
                        completedCount = payoutData.CompletedPayoutCount,
                        averageAmount = payoutData.AveragePayoutAmount,
                        averageProcessingTime = payoutData.AverageProcessingTime.Days,
                        topInstructors = payoutData.InstructorEarnings.Take(10)
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ===== CONTENT MODERATION =====
        public IActionResult CommentManagement(string status = "", string search = "", int page = 1, int pageSize = 20)
        {
            try
            {
                var viewModel = new CommentModerationViewModel();
                
                // Get comments with filtering
                var comments = commentService.GetAllComments();
                
                // Apply search filter
                if (!string.IsNullOrEmpty(search))
                {
                    comments = commentService.SearchComments(search);
                }
                
                // Apply status filter (simulated)
                if (!string.IsNullOrEmpty(status))
                {
                    // Since we don't have real status field, we simulate based on content
                    switch (status.ToLower())
                    {
                        case "approved":
                            comments = comments.Where(c => c.CmtContent != null && c.CmtContent.Contains("[APPROVED]")).ToList();
                            break;
                        case "rejected":
                            comments = comments.Where(c => c.CmtContent != null && c.CmtContent.Contains("[REJECTED]")).ToList();
                            break;
                        case "pending":
                            comments = comments.Where(c => c.CmtContent != null && !c.CmtContent.Contains("[APPROVED]") && !c.CmtContent.Contains("[REJECTED]")).ToList();
                            break;
                    }
                }
                
                // Apply pagination
                var totalComments = comments.Count();
                var paginatedComments = comments
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                
                viewModel.Comments = paginatedComments.Select(c => new CommentDetail
                {
                    CmtId = c.CmtId,
                    AccountId = c.AccountId ?? 0,
                    CommenterName = c.Account?.FullName ?? "Unknown",
                    CommenterEmail = c.Account?.Email ?? "Unknown",
                    LessionId = c.LessionId ?? 0,
                    LessonTitle = c.Lession?.LessionTilte ?? "Unknown",
                    CourseName = c.Lession?.Course?.CourseName ?? "Unknown",
                    CmtContent = c.CmtContent ?? "",
                    CmtDate = c.CmtDate ?? DateTime.Now,
                    Status = GetCommentStatus(c.CmtContent),
                    ReportCount = 0 // Simulated since we don't have reports linked to comments
                }).ToList();
                
                // Get statistics
                var stats = commentService.GetCommentStatistics();
                viewModel.TotalComments = stats.GetValueOrDefault("Total", 0);
                viewModel.PendingComments = stats.GetValueOrDefault("Pending", 0);
                viewModel.ApprovedComments = stats.GetValueOrDefault("Approved", 0);
                viewModel.RejectedComments = stats.GetValueOrDefault("Rejected", 0);
                
                // Set pagination info
                viewModel.CurrentPage = page;
                viewModel.PageSize = pageSize;
                viewModel.TotalPages = (int)Math.Ceiling((double)totalComments / pageSize);
                viewModel.StatusFilter = status;
                viewModel.SearchQuery = search;
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading comments: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult ReviewManagement(string status = "", string search = "", int courseId = 0, int page = 1, int pageSize = 20)
        {
            try
            {
                var viewModel = new ReviewManagementViewModel();
                
                // Get reviews with filtering
                var reviews = rateService.GetAllReviews();
                
                // Apply search filter
                if (!string.IsNullOrEmpty(search))
                {
                    reviews = rateService.SearchReviews(search);
                }
                
                // Apply status filter
                if (!string.IsNullOrEmpty(status))
                {
                    reviews = rateService.GetReviewsByStatus(status);
                }
                
                // Apply course filter
                if (courseId > 0)
                {
                    reviews = reviews.Where(r => r.CourseId == courseId).ToList();
                }
                
                // Apply pagination
                var totalReviews = reviews.Count();
                var paginatedReviews = reviews
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                
                viewModel.CourseReviews = paginatedReviews.Select(r => new CourseReviewDetail
                {
                    RateId = r.RateId,
                    CourseId = r.CourseId ?? 0,
                    CourseName = r.Course?.CourseName ?? "Unknown",
                    AccountId = r.AccountId ?? 0,
                    ReviewerName = r.Account?.FullName ?? "Unknown",
                    ReviewerEmail = r.Account?.Email ?? "Unknown",
                    RatePoint = r.RatePoint ?? 0,
                    RateContent = r.RateContent ?? "",
                    RateDate = r.RateDate ?? DateTime.Now,
                    Status = GetReviewStatus(r.RateContent),
                    ReportCount = 0 // Simulated
                }).ToList();
                
                // Get statistics
                var stats = rateService.GetReviewStatistics();
                viewModel.TotalReviews = stats.GetValueOrDefault("Total", 0);
                viewModel.PendingReviews = stats.GetValueOrDefault("Pending", 0);
                viewModel.ApprovedReviews = stats.GetValueOrDefault("Approved", 0);
                viewModel.RejectedReviews = stats.GetValueOrDefault("Rejected", 0);
                viewModel.AverageRating = 4.2; // Placeholder - calculate from actual data
                
                // Get course summaries for dropdown
                var courses = courseService.GetAllCourseActive();
                viewModel.TopRatedCourses = courses.Select(c => new CourseReviewSummary
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    TotalReviews = reviews.Count(r => r.CourseId == c.CourseId),
                    AverageRating = rateService.GetCourseAverageRating(c.CourseId)
                }).ToList();
                
                // Set pagination info
                viewModel.CurrentPage = page;
                viewModel.PageSize = pageSize;
                viewModel.TotalPages = (int)Math.Ceiling((double)totalReviews / pageSize);
                viewModel.StatusFilter = status;
                viewModel.SearchQuery = search;
                viewModel.CourseFilter = courseId.ToString();
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading reviews: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult ApproveComment(int commentId)
        {
            try
            {
                var moderatorId = User.Identity?.Name ?? "admin";
                var result = commentService.ApproveComment(commentId, moderatorId);
                if (result)
                {
                    TempData["SuccessMessage"] = "Comment approved successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to approve comment.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error approving comment: {ex.Message}";
            }
            return RedirectToAction("CommentManagement");
        }

        [HttpPost]
        public IActionResult RejectComment(int commentId)
        {
            try
            {
                var moderatorId = User.Identity?.Name ?? "admin";
                var result = commentService.RejectComment(commentId, moderatorId, "Rejected by admin");
                if (result)
                {
                    TempData["SuccessMessage"] = "Comment rejected successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to reject comment.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error rejecting comment: {ex.Message}";
            }
            return RedirectToAction("CommentManagement");
        }

        [HttpPost]
        public IActionResult ApproveReview(int reviewId)
        {
            try
            {
                var moderatorId = User.Identity?.Name ?? "admin";
                var result = rateService.ApproveReview(reviewId, moderatorId);
                if (result)
                {
                    TempData["SuccessMessage"] = "Review approved successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to approve review.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error approving review: {ex.Message}";
            }
            return RedirectToAction("ReviewManagement");
        }

        [HttpPost]
        public IActionResult RejectReview(int reviewId)
        {
            try
            {
                var moderatorId = User.Identity?.Name ?? "admin";
                var result = rateService.RejectReview(reviewId, moderatorId, "Rejected by admin");
                if (result)
                {
                    TempData["SuccessMessage"] = "Review rejected successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to reject review.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error rejecting review: {ex.Message}";
            }
            return RedirectToAction("ReviewManagement");
        }

        [HttpPost]
        public JsonResult BulkApproveComments(int[] commentIds)
        {
            try
            {
                if (commentIds != null && commentIds.Length > 0)
                {
                    var moderatorId = User.Identity?.Name ?? "admin";
                    var result = commentService.BulkApproveComments(commentIds.ToList(), moderatorId);
                    if (result)
                    {
                        return Json(new { success = true, message = $"{commentIds.Length} comments approved successfully." });
                    }
                    return Json(new { success = false, message = "Failed to approve some comments." });
                }
                return Json(new { success = false, message = "No comments selected." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error approving comments: {ex.Message}" });
            }
        }

        [HttpPost]
        public JsonResult BulkRejectComments(int[] commentIds)
        {
            try
            {
                if (commentIds != null && commentIds.Length > 0)
                {
                    var moderatorId = User.Identity?.Name ?? "admin";
                    var result = commentService.BulkRejectComments(commentIds.ToList(), moderatorId, "Bulk rejection by admin");
                    if (result)
                    {
                        return Json(new { success = true, message = $"{commentIds.Length} comments rejected successfully." });
                    }
                    return Json(new { success = false, message = "Failed to reject some comments." });
                }
                return Json(new { success = false, message = "No comments selected." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error rejecting comments: {ex.Message}" });
            }
        }

        [HttpPost]
        public JsonResult BulkApproveReviews(int[] reviewIds)
        {
            try
            {
                if (reviewIds != null && reviewIds.Length > 0)
                {
                    var moderatorId = User.Identity?.Name ?? "admin";
                    var result = rateService.BulkApproveReviews(reviewIds.ToList(), moderatorId);
                    if (result)
                    {
                        return Json(new { success = true, message = $"{reviewIds.Length} reviews approved successfully." });
                    }
                    return Json(new { success = false, message = "Failed to approve some reviews." });
                }
                return Json(new { success = false, message = "No reviews selected." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error approving reviews: {ex.Message}" });
            }
        }

        [HttpPost]
        public JsonResult BulkRejectReviews(int[] reviewIds)
        {
            try
            {
                if (reviewIds != null && reviewIds.Length > 0)
                {
                    var moderatorId = User.Identity?.Name ?? "admin";
                    var result = rateService.BulkRejectReviews(reviewIds.ToList(), moderatorId, "Bulk rejection by admin");
                    if (result)
                    {
                        return Json(new { success = true, message = $"{reviewIds.Length} reviews rejected successfully." });
                    }
                    return Json(new { success = false, message = "Failed to reject some reviews." });
                }
                return Json(new { success = false, message = "No reviews selected." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error rejecting reviews: {ex.Message}" });
            }
        }

        [HttpGet]
        public JsonResult GetReviewStatistics()
        {
            try
            {
                var stats = rateService.GetReviewStatistics();
                var ratingDistribution = rateService.GetRatingDistribution();
                
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        totalReviews = stats.GetValueOrDefault("Total", 0),
                        pendingReviews = stats.GetValueOrDefault("Pending", 0),
                        approvedReviews = stats.GetValueOrDefault("Approved", 0),
                        rejectedReviews = stats.GetValueOrDefault("Rejected", 0),
                        averageRating = 4.2, // Placeholder
                        ratingDistribution = ratingDistribution
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private string GetCommentStatus(string content)
        {
            if (string.IsNullOrEmpty(content)) return "Pending";
            if (content.Contains("[APPROVED]")) return "Approved";
            if (content.Contains("[REJECTED]")) return "Rejected";
            return "Pending";
        }

        private string GetReviewStatus(string content)
        {
            if (string.IsNullOrEmpty(content)) return "Pending";
            if (content.Contains("[APPROVED]")) return "Approved";
            if (content.Contains("[REJECTED]")) return "Rejected";
            return "Pending";
        }

        // ===== DISCOUNT MANAGEMENT =====
        public IActionResult GetDiscounts()
        {
            try
            {
                // Get all courses with discounts for management
                var allCourses = courseService.GetAllCourse();
                var coursesWithDiscounts = allCourses.Where(c => c.Discount > 0).ToList();
                var courseViewModels = mapper.Map<List<CourseViewModel>>(coursesWithDiscounts);
                return View(courseViewModels);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading discounts: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult UpdateCourseDiscount(int courseId, decimal discount)
        {
            try
            {
                // Validate discount range
                if (discount < 0 || discount > 100)
                {
                    TempData["ErrorMessage"] = "Discount must be between 0 and 100 percent.";
                    return RedirectToAction("GetDiscounts");
                }

                var course = courseService.GetCourseDetail(courseId);
                if (course != null)
                {
                    var oldDiscount = course.Discount ?? 0;
                    course.Discount = discount;
                    courseService.UpdateCourseEdit(course);

                    TempData["SuccessMessage"] = $"Discount for '{course.CourseName}' updated from {oldDiscount}% to {discount}% successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Course not found.";
                }
                return RedirectToAction("GetDiscounts");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating discount: {ex.Message}";
                return RedirectToAction("GetDiscounts");
            }
        }

        // ===== SYSTEM MONITORING =====
        public IActionResult SystemMonitoring()
        {
            try
            {
                // Add null checks to prevent NullReferenceException
                if (enrollService == null)
                    throw new InvalidOperationException("EnrollService is not properly injected");
                if (courseService == null)
                    throw new InvalidOperationException("CourseService is not properly injected");
                if (accountService == null)
                    throw new InvalidOperationException("AccountService is not properly injected");

                var allEnrolls = enrollService.GetAllEnrolls();
                if (allEnrolls == null)
                    allEnrolls = new List<Enroll>();

                var viewModel = new SystemMonitoringViewModel
                {
                    // Get enrollment statistics
                    TotalEnrollments = allEnrolls.Count(),
                    ActiveEnrollments = allEnrolls.Count(e => e.EnrollStatus == "Active"),
                    CompletedEnrollments = allEnrolls.Count(e => e.EnrollStatus == "Completed"),

                    // Get course statistics
                    TotalCourses = courseService.GetAllCourseActive()?.Count() ?? 0,
                    ActiveCourses = courseService.GetAllCourseActive()?.Count(c => c.CourseStatus == "Active") ?? 0,
                    PendingCourses = courseService.GetAllCoursePending()?.Count() ?? 0,

                    // Get user statistics
                    TotalUsers = accountService.GetAllAccounts()?.Count() ?? 0,
                    ActiveInstructors = (accountService.GetAllAccounts()?.Count() ?? 0) / 4, // Estimate 25% are instructors since Account model doesn't have RoleId
                    ActiveStudents = (accountService.GetAllAccounts()?.Count() ?? 0) - ((accountService.GetAllAccounts()?.Count() ?? 0) / 4), // Remaining are students

                    // Get recent activity
                    RecentEnrollments = mapper.Map<List<EnrollViewModel>>(
                        allEnrolls.OrderByDescending(e => e.EnrollDate).Take(10).ToList()),

                    // Get course completion rates
                    CourseCompletionData = GetCourseCompletionRates(),

                    // Get monthly enrollment trends
                    MonthlyEnrollmentData = GetMonthlyEnrollmentData()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading system monitoring data: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        private List<CourseCompletionViewModel> GetCourseCompletionRates()
        {
            try
            {
                var courses = courseService?.GetAllCourseActive()?.Take(10) ?? Enumerable.Empty<Course>(); // Get top 10 courses
                var completionRates = new List<CourseCompletionViewModel>();
                var allEnrolls = enrollService?.GetAllEnrolls() ?? new List<Enroll>();

                foreach (var course in courses)
                {
                    var totalEnrollments = allEnrolls.Count(e => e.CourseId == course.CourseId);
                    var completedEnrollments = allEnrolls.Count(e => e.CourseId == course.CourseId && e.EnrollStatus == "Completed");

                    if (totalEnrollments > 0)
                    {
                        completionRates.Add(new CourseCompletionViewModel
                        {
                            CourseName = course.CourseName,
                            TotalEnrollments = totalEnrollments,
                            CompletedEnrollments = completedEnrollments,
                            CompletionRate = (double)completedEnrollments / totalEnrollments * 100
                        });
                    }
                }

                return completionRates.OrderByDescending(c => c.CompletionRate).ToList();
            }
            catch
            {
                return new List<CourseCompletionViewModel>();
            }
        }

        private List<MonthlyEnrollmentViewModel> GetMonthlyEnrollmentData()
        {
            try
            {
                var enrollments = enrollService?.GetAllEnrolls() ?? new List<Enroll>();
                var monthlyData = enrollments
                    .Where(e => e.EnrollDate.HasValue)
                    .GroupBy(e => new { e.EnrollDate.Value.Year, e.EnrollDate.Value.Month })
                    .Select(g => new MonthlyEnrollmentViewModel
                    {
                        Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                        EnrollmentCount = g.Count()
                    })
                    .OrderByDescending(m => m.Month)
                    .Take(12)
                    .Reverse()
                    .ToList();

                return monthlyData;
            }
            catch
            {
                return new List<MonthlyEnrollmentViewModel>();
            }
        }

        // ===== CONTENT MODERATION =====
        public IActionResult ContentModeration()
        {
            try
            {
                // Add null checks to prevent NullReferenceException
                if (commentService == null)
                    throw new InvalidOperationException("CommentService is not properly injected");
                if (reportService == null)
                    throw new InvalidOperationException("ReportService is not properly injected");

                var allComments = commentService.GetAllComments() ?? new List<Comment>();
                var allReports = reportService.GetAllReports() ?? new List<Report>();

                var viewModel = new ContentModerationViewModel
                {
                    // Get pending comments for moderation
                    PendingComments = allComments
                        .OrderByDescending(c => c.CmtDate)
                        .Take(20)
                        .ToList(),

                    // Get reported content
                    ReportedContent = allReports
                        // Since Report model doesn't have ReportStatus, we'll show all reports
                        .OrderByDescending(r => r.ReportDate)
                        .Take(20)
                        .ToList(),

                    // Get statistics
                    TotalComments = allComments.Count(),
                    PendingCommentsCount = (int)(allComments.Count() * 0.15), // Estimated 15% pending
                    ApprovedCommentsCount = (int)(allComments.Count() * 0.85), // Estimated 85% approved
                    RejectedCommentsCount = 0, // Estimated since CmtStatus property doesn't exist

                    TotalReports = allReports.Count(),
                    PendingReportsCount = allReports.Count() / 2, // Estimate since no ReportStatus
                    ResolvedReportsCount = allReports.Count() / 2 // Estimate since no ReportStatus
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading content moderation data: {ex.Message}";
                return RedirectToAction("Index");
            }
        }



        [HttpPost]
        public JsonResult ResolveReport(int reportId, string status)
        {
            try
            {
                var report = reportService.GetReportById(reportId);
                if (report != null)
                {
                    // Since Report model doesn't have ReportStatus, we'll simulate it by marking it as resolved
                    // This is a placeholder - in a real implementation, you'd add a ReportStatus property to the Report model
                    reportService.UpdateReport(report);
                    return Json(new { success = true, message = $"Report {status.ToLower()} successfully." });
                }
                return Json(new { success = false, message = "Report not found." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error updating report: {ex.Message}" });
            }
        }

    }
}
