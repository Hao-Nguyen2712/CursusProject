using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cursus.Application.Admin;
using Cursus.Application.Analyze;
using Cursus.Application.Category;
using Cursus.Application.Instructor;
using Cursus.Application.Student;
using Cursus.Application.AdminDashBoard;
using Cursus.Domain.Models;
using Cursus.MVC.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Cursus.Application.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Cursus.MVC.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Cursus.MVC.Service;
using Cursus.Application;
using Cursus.Application.Account;
using Cursus.MVC.Areas.Identity.Data;


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
        private readonly IAccountService _accountService;
        private readonly IAdminService adminService;
        private readonly IEmailSender mailSender;
        private readonly IAccountService accountService;
        private readonly IEmailSender _emailSender;
        private readonly ICourseService courseService;
        private readonly UserManager<CursusMVCUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<CursusMVCUser> _signInManager;

        public AdminController(ICategoryService _categoryService, IInstructorService _instructorService, IStudentService _studentService, IAnalyzeService _analyzeService, IAdminDashBoardService adminDashBoardService, IAdminService _adminService, IMapper _mapper, IEmailSender emailSender, ICourseService courseService, IAccountService accountService, RoleManager<IdentityRole> roleManager, UserManager<CursusMVCUser> userManager, SignInManager<CursusMVCUser> signInManager, IEmailSender _mailSender)

        {
            this.categoryService = _categoryService;
            this.studentService = _studentService;
            this.instructorService = _instructorService;
            this.analyzeService = _analyzeService;
            _adminDashBoardService = adminDashBoardService;
            this.mapper = _mapper;
            this.adminService = _adminService;

            this.mailSender = _mailSender;
            this.accountService = _accountService;

            _emailSender = emailSender;
            this.courseService = courseService;
            _accountService = accountService;
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

        public IActionResult ApproveCourse(int id)
        {
            this.adminService.ApproveCourse(id);
            Course course = courseService.GetCourseDetail(id);
            Account account = course.Account;
            _emailSender.SendEmailAsync(account.Email, "Notification Course", Service.EmailSender.EmailNotiConfirmCourse(account.FullName, course.CourseName, "Approved"));
            return RedirectToAction("GetCourseApprove");
        }

        [Route("Admin/DetailsCourseConfirm/{id:int}")]
        public IActionResult DetailsCourseConfirm(int id)
        {
            Course course = courseService.GetCourseDetail(id);
            Account account = course.Account;
            ViewBag.Account= account;
            List<Course> list = new List<Course>();
            list.Add(course);
            List<CourseViewModel> courses = mapper.Map<List<CourseViewModel>>(list);
            return View(courses);
        }

        [Route("Admin/RejectCourse/{id:int}")]

        public IActionResult RejectCourse(int id)
        {
            this.adminService.RejectCourse(id);
            Course course = courseService.GetCourseDetail(id);
            Account account = course.Account;
            _emailSender.SendEmailAsync(account.Email, "Notification Course", Service.EmailSender.EmailNotiConfirmCourse(account.FullName, course.CourseName, "Rejected"));
            return RedirectToAction("GetCourseApprove");
        }
        public IActionResult GetListAccount()
        {
            var list = adminService.getListAccount();
            List<AccountViewModel> account = mapper.Map<List<AccountViewModel>>(list);
            return View(account);
        }
        [Route("Admin/BlockAccount/{id:int}")]

        public IActionResult BlockAccount(int id)
        {
            this.adminService.BlockAccount(id);
            Account account = _accountService.GetAccountByAccountID(id);
            _emailSender.SendEmailAsync(account.Email + "", "Notification Account", Service.EmailSender.EmailNotiAccount(account.FullName, "blocked"));
            return RedirectToAction("GetListAccount");
        }

        [Route("Admin/UnBlockAccount/{id:int}")]

        public IActionResult UnBlockAccount(int id)
        {
            this.adminService.UnBlockAccount(id);
            Account account = _accountService.GetAccountByAccountID(id);
            _emailSender.SendEmailAsync(account.Email + "", "Notification Account", Service.EmailSender.EmailNotiAccount(account.FullName, "unblock"));
            return RedirectToAction("GetListAccount");
        }

        public IActionResult GetListInstructorRegis()
        {
            var list = adminService.getListInstructorRegis();
            List<AccountViewModel> account = mapper.Map<List<AccountViewModel>>(list);
            return View(account);
        }

        [Route("Admin/ApproveInstructorRegis/{id:int}")]
        public IActionResult ApproveInstructorRegis(int id)
        {
            this.adminService.ApproveInstructorRegis(id);
            Account account = _accountService.GetAccountByAccountID(id);
            string email = _accountService.getEmail(id);
            var user = _userManager.FindByEmailAsync(email).Result;
            if (user != null)
            {
                _userManager.RemoveFromRoleAsync(user, "Student").Wait();
                if (!_roleManager.RoleExistsAsync("Instructor").Result)
                {
                    _roleManager.CreateAsync(new IdentityRole("Instructor")).Wait();
                }
                _userManager.AddToRoleAsync(user, "Instructor").Wait();
                _emailSender.SendEmailAsync(
                    account.Email,
                    "Notification Regis",
                    Service.EmailSender.EmailNotiConfirmInstruction(account.FullName, " Approved")
                ).Wait();
            }
            return RedirectToAction("GetListInstructorRegis");
        }

        [Route("Admin/RejectInstructorRegis/{id:int}")]

        public IActionResult RejectInstructorRegis(int id)
        {
            this.adminService.RejectInstructorRegis(id);
            Account account = _accountService.GetAccountByAccountID(id);
            _emailSender.SendEmailAsync(account.Email + "", "Notification Regis", Service.EmailSender.EmailNotiConfirmInstruction(account.FullName, "Rejected"));
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
        public IActionResult RemoveStudent(int id)
        {
            var student = studentService.RemoveStudent(id);
            var account = accountService.GetAccountByAccountID(id);
            // var email =  account.Email;
            if (student)
            {
                //   var student =  studentService.
                mailSender.SendEmailAsync(
                account.Email,
                 "Confirm Admin",
                 Service.EmailSender.AdminNotification(account.Username)
               );
                TempData["SuccessMessage"] = "Student remove successfully!";
                return RedirectToAction("GetStudent");
            }
            TempData["ErrorMessage"] = "Failed to remove Student.";
            return Redirect("/404");
        }
        public IActionResult RemoveInstructor(int id)
        {
            var instructor = instructorService.RemoveInstructor(id);
            var account = accountService.GetAccountByAccountID(id);
            if (instructor)
            {
                mailSender.SendEmailAsync(
              account.Email,
               "Confirm Admin",
               Service.EmailSender.AdminNotification(account.Username)
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



    }
}