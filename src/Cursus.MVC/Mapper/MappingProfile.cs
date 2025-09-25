using AutoMapper;
using Cursus.Domain.Models;
using Cursus.Domain.ViewModels;
using Cursus.MVC.Models;
using Cursus.MVC.ViewModels;


namespace Cursus.MVC.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountViewModel>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Cart, CartViewModel>().ReverseMap();
            CreateMap<Cursus.Domain.Models.Comment, CommentViewModel>().ReverseMap();
            CreateMap<Course, CourseViewModel>().ForMember(dest => dest.AccountVM, opt => opt.MapFrom(src => src.Account))
                                                .ForMember(dest => dest.CategoryVM, opt => opt.MapFrom(src => src.Category)).ReverseMap();
            CreateMap<Discount, DiscountViewModel>().ReverseMap();
            CreateMap<Enroll, EnrollViewModel>().ReverseMap();
            CreateMap<Lesson, LessonViewModel>().ReverseMap();
            CreateMap<Rate, RateViewModel>().ReverseMap();
            CreateMap<Report, ReportViewModel>().ReverseMap();
            CreateMap<Otp, OtpViewModel>().ReverseMap();
            CreateMap<Trading, TradingViewModel>().ReverseMap();

            CreateMap<DashBoard, DashBoardViewModel>().ReverseMap();
            CreateMap<AdminDashBoard, AdminDashBoardViewModel>().ReverseMap();
            CreateMap<HomePageView, HomePageViewViewModel>().ReverseMap();
            CreateMap<Subscribe, SubscriseViewModel>().ReverseMap();
            CreateMap<ProFileView, ProFileViewViewModel>().ReverseMap();

        }
    }
}
