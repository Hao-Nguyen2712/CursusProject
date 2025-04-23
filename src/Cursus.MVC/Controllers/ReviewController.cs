using AutoMapper;
using Cursus.Application.Review;
using Cursus.Infrastructure.Review;
using Cursus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cursus.MVC.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;
        public ReviewController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        public IActionResult ViewReview()
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var review = _reviewService.GetAllReview(userID, "");

            return View(review);

        }
        public IActionResult SearchReviews(string searchTerm)
        {
            ClaimsPrincipal claims = this.User;
            var userID = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var reviews = _reviewService.GetAllReview(userID, searchTerm);

            // Kiểm tra nếu reviews là null hoặc không có dữ liệu, trả về mảng rỗng
            if (reviews == null || reviews.Count == 0)
            {
                return Json(new List<Application.Models.ViewReviewViewModel>());
            }
            return Json(reviews);
        }
    }
}
