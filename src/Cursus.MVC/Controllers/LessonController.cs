using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Cursus.Domain.Models;
using Cursus.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Cursus.MVC.Areas.Identity.Data;
using System.Security.Claims;


namespace Cursus.MVC.Controllers
{
    public class LessonController : Controller
    {
        private readonly Cursus.Application.ILessonService _lessonService;
        private readonly UserManager<CursusMVCUser> _userManager;
        private readonly IUserStore<CursusMVCUser> _userStore;

        public LessonController(UserManager<CursusMVCUser> userManager, IUserStore<CursusMVCUser> userStore, Cursus.Application.ILessonService lessonService)
        {

            _userManager = userManager;
            _userStore = userStore;
            _lessonService = lessonService;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult AddLesson()
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            return View();
        }

        [HttpPost]
        public IActionResult AddLesson(Cursus.Domain.Models.Lesson lession)
        {
            return View(lession);
        }

        public IActionResult GetSmallestLessonId(int courseID)
        {
            var smallestLessonId = _lessonService.GetLessonID(courseID);
            if(smallestLessonId != 0 ){
                return RedirectToAction("GetCourse", "Course", new { courseID = courseID, lessonID = smallestLessonId.Value });
            }
            else{
                return RedirectToAction("GetCourseDetail", "Course", new {id = courseID});
                
            } 
            // var smallestLessonId = _lessonService.GetLessonID(courseID);
            // if (smallestLessonId.HasValue)
            // {
            //     return Json(new { lessonId = smallestLessonId.Value });
            // }
            // else
            // {
            //     return Json(new { lessonId = (int?)null });
            // }
        }

    }
}