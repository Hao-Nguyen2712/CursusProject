using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cursus.Application.Account;
using Cursus.Application.Cart;
using Cursus.Application.Category;
using Cursus.Domain.Models;
using Cursus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cursus.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ICartService _cartService;
        private readonly IAccountService _accountService;

        public CategoryController(ICategoryService categoryService, ICartService cartService, IAccountService accountService)
        {
            _categoryService = categoryService;
            _cartService = cartService;
            _accountService = accountService;
        }

        public IActionResult GetCategory(int CategoryId)
        {
            var courses = _categoryService.GetCategory(CategoryId);
            
            // Get the category information to display the name
            var category = _categoryService.FindCategoryById(CategoryId);

            var categoryViewModel = new CategoryViewModel
            {
                CategoryId = CategoryId,
                CategoryName = category?.CategoryName ?? "Unknown Category",
                CoursesVM = courses.Select(course => new CourseViewModel
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    CourseShortDes = course.CourseShortDes,
                    CourseDescription = course.CourseDescription,
                    CourseWlearn = course.CourseWlearn,
                    CourseRequirement = course.CourseRequirement,
                    CourseAvatar = course.CourseAvatar,
                    CourseDate = course.CourseDate,
                    CourseMoney = course.CourseMoney,
                    CourseStatus = course.CourseStatus,
                    CourseProcess = course.CourseProcess,
                    // DiscountId = course.DiscountId,
                    CategoryId = course.CategoryId
                }).ToList()
            };
            return View("GetCategory", categoryViewModel);
        }
    }
}