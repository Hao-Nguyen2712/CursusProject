using AutoMapper;
using Cursus.Application.Account;
using Cursus.Application.Comment;
using Cursus.Domain.Models;
using Cursus.Infrastructure.Comment;
using Cursus.MVC.Areas.Identity.Data;
using Cursus.MVC.Models;
using Cursus.MVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Cursus.Application.Progress;
using Cursus.Application;
using Cursus.Application.Enroll;
namespace Cursus.MVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly UserManager<CursusMVCUser> _userManager;
        private readonly IUserStore<CursusMVCUser> _userStore;
        private readonly IProgressService _progressService;
        private readonly ICourseService _courseService;
        private readonly IEnrollService _enrollService;
        private readonly ILessonService _lessonService;

        public CommentController(ICommentService commentService, IMapper mapper, IAccountService accountService, UserManager<CursusMVCUser> userManager, IUserStore<CursusMVCUser> userStore, IProgressService progressService, ICourseService courseService, IEnrollService enrollService, ILessonService lessonService)
        {
            _commentService = commentService;
            _mapper = mapper;
            _accountService = accountService;
            _userManager = userManager;
            _userStore = userStore;
            _progressService = progressService;
            _courseService = courseService;
            _enrollService = enrollService;
            _lessonService = lessonService;
        }

        [HttpPost]
        public IActionResult AddComment(CommentResponViewModel obj)
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int accountId = _accountService.GetAccountIDByUserID(userID);

            Account account = _accountService.GetAccountByAccountID(accountId);
            AccountViewModel accountViewModel = _mapper.Map<AccountViewModel>(account);

            CommentViewModel comment = new CommentViewModel();
            comment.CmtLevel = 1;
            comment.CmtContent = obj.content;
            comment.LessionId = obj.LessonId;
            comment.AccountId = accountId;
            comment.CmtDate = DateTime.Now;
            comment.CmtReply = "0";
            // map
            Cursus.Domain.Models.Commnent commentModel = _mapper.Map<Cursus.Domain.Models.Commnent>(comment);
            commentModel = _commentService.addComment(commentModel);
            comment.CmtId = commentModel.CmtId;

            // update comment count in lesson
            Lesson lesson = _lessonService.GetLesson(obj.LessonId);
            lesson.LessionComments = lesson.LessionComments + 1;
            lesson = _lessonService.UpadateLesson(lesson);


            return Json(new {LessonID = obj.LessonId, Comment = comment, AccountViewModel = accountViewModel });
        }

        [HttpPost]
        public IActionResult AddReplyComment(CommentResponViewModel obj)
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int accountId = _accountService.GetAccountIDByUserID(userID);

            Account account = _accountService.GetAccountByAccountID(accountId);
            AccountViewModel accountViewModel = _mapper.Map<AccountViewModel>(account);

            CommentViewModel comment = new CommentViewModel();
            comment.CmtLevel = 2;
            comment.CmtContent = obj.content;
            comment.LessionId = obj.LessonId;
            comment.AccountId = accountId;
            comment.CmtDate = DateTime.Now;
            comment.CmtReply = obj.reply;
            // map
            Cursus.Domain.Models.Commnent commentModel = _mapper.Map<Cursus.Domain.Models.Commnent>(comment);
            commentModel = _commentService.addComment(commentModel);
            comment.CmtId = commentModel.CmtId;
            // update comment count in lesson
            Lesson lesson = _lessonService.GetLesson(obj.LessonId);
            lesson.LessionComments = lesson.LessionComments + 1;
            lesson = _lessonService.UpadateLesson(lesson);

            return Json(new { LessonID = obj.LessonId, Comment = comment, AccountViewModel = accountViewModel });
        }

        [HttpPost]
        public IActionResult FinishStudy(int lessonID, int courseID)
         {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int accountID = _accountService.GetAccountIDByUserID(userID);
            
            Domain.Models.Progress progress = _progressService.GetProgressByAccountIDAndLessonID(userID, lessonID);
            if(progress != null)
            {
                return Json("False");
            }
            progress = new Domain.Models.Progress();
            progress.AccId = userID;
            progress.LessonId = lessonID;
            progress.Finish = "True";
            _progressService.AddProgress(progress);
            // Dem so luong lesson trong course
            int countLesson = 0;
            List<int> listLessonID = _progressService.GetListLessonIDByCourseID(courseID);
            foreach(var item in listLessonID) {
                int count = _progressService.coutProgressByAccountID(item, userID);
                if(count > 0)
                {
                    countLesson++;
                }
            }

            if(countLesson == listLessonID.Count)
            {
                // update finish course in enroll
                Domain.Models.Enroll enroll = _enrollService.UpdateFinishByAccountID(accountID, courseID);
                if(enroll == null)
                {
                    return Json("False");
                }
            }

            return Json("True");
        }

        [HttpPost]
        public IActionResult NotFinishStudy(int lessonID, int courseID)
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            int accountID = _accountService.GetAccountIDByUserID(userID);
            // luu vao bang progress (accID, lessID, progress = true)
            // Dem so luong lesson trong course
            // so sanh so luong lesson trong course voi so luong lesson da hoan thanh
            // return Json(new { CommentID = commentId });
            int count = _progressService.DeleteProgress(userID, lessonID);
            if (count == 0)
            {
                return Json("False");
            }
            return Json("True");
        }   
    }
}
