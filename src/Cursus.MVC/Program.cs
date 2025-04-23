using Cursus.Infrastructure;
using Cursus.Application;
using Cursus.Domain.Models;
using AutoMapper;
using Cursus.MVC.Mapper;
using Cursus.Application.SearchInstructor;
using Cursus.Infrastructure.SearchInstructor;
using Cursus.MVC.Controllers;
using Cursus.Application.Cart;
using Cursus.Infrastructure.Cart;
using Cursus.MVC.ViewModels;
using Cursus.MVC.Data;
using Microsoft.EntityFrameworkCore;
using Cursus.MVC.Areas.Identity.Data;
using Cursus.Application.Category;
using Cursus.Infrastructure.Category;
using Cursus.Infrastructure.Student;
using Microsoft.Extensions.DependencyInjection;
using Cursus.Application.Student;
using Cursus.Application.Instructor;
using Cursus.Infrastructure.Instructor;
using Cursus.Application.Analyze;
using Cursus.Infrastructure.Analyze;
using Microsoft.AspNetCore.StaticFiles;
using Cursus.Application.Enroll;
using Cursus.Infrastructure.Enroll;
using Cursus.Application.Account;
using Cursus.Application.Report;
using Cursus.Infrastructure.Report;
using Cursus.Application.DashBoard;
using Cursus.Application.Subscrise;
using Cursus.Infrastructure.Subscrise;
using Cursus.Application.AdminDashBoard;
using Cursus.MVC.Service;
using Microsoft.AspNetCore.Identity.UI.Services;
using Cursus.Application.Admin;
using Cursus.Infrastructure.Admin;
using Cursus.MVC.Services;
using Azure.Storage.Blobs;
using Cursus.Application.Comment;
using Cursus.Infrastructure.Comment;
using Microsoft.AspNetCore.Identity;
using Cursus.Application.Progress;
using Cursus.Infrastructure.Progress;
using Cursus.Application.Credits;
using Cursus.Infrastructure.Credits;
using Cursus.Application.Review;
using Cursus.Infrastructure.Review;
using Cursus.Application.Earning;
using Cursus.Infrastructure.Earning;
using Cursus.Application.Payout;
using Cursus.Infrastructure.Payout;
using Cursus.Application.Certificate;
using Cursus.Application.Subscription;
using Cursus.Infrastructure.Subscription;

using Cursus.Application.Profile;

namespace Cursus.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("CursusMVCContextConnection") ?? throw new InvalidOperationException("Connection string 'CursusMVCContextConnection' not found.");
            var config = builder.Configuration;

            builder.Services.AddDbContext<CursusMVCContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<CursusMVCUser>(options => options.SignIn.RequireConfirmedAccount = true)
             .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<CursusMVCContext>();

        
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<CursusDBContext>();

            builder.Services.AddRazorPages();

            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();

            builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
            builder.Services.AddScoped<IInstructorService, InstructorService>();

            builder.Services.AddScoped<IAnalyzeRepository, AnalyzeRepository>();
            builder.Services.AddScoped<IAnalyzeService, AnalyzeService>();

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            builder.Services.AddSingleton(provider);

            builder.Services.AddScoped<ILessonService, LessonService>();
            builder.Services.AddScoped<ILessonRepository, LessonRepository>();

            builder.Services.AddScoped<IEnrollService, EnrollService>();
            builder.Services.AddScoped<IEnrollRepository, EnrollRepository>();

            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddScoped<ICourseRepository, CourseRepository>();

   
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            builder.Services.AddScoped<ICourseService, CourseService>();

            builder.Services.AddScoped<IRateRepository, RateRepository>();
            builder.Services.AddScoped<IRateService, RateService>();

            builder.Services.AddScoped<IEnrollRepository, EnrollRepository>();
            builder.Services.AddScoped<IEnrollService, EnrollService>();

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ICommentRepository, CommentReposidtory>();

            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IReportService, ReportService>();

            builder.Services.AddScoped<ISubscriseRepository, SubscriseRepository>();
            builder.Services.AddScoped<ISubscriseService, SubscriseService>();

            builder.Services.AddScoped<IDashBoardService, DashBoardService>();

            builder.Services.AddScoped<IAdminDashBoardService, AdminDashBoardService>();

            builder.Services.AddScoped<IInstructorCourseService, InstructorCourseService>();
            builder.Services.AddScoped<IInstructorCourseRepository, InstructorCourseRepository>();

            builder.Services.AddScoped<IHomePageService, HomePageService>();

            builder.Services.AddScoped<ISearchInstructorRepository, SearchInstructorRepository>();
            builder.Services.AddScoped<ISearchInstructorService, SearchInstructorService>();

            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICartService, CartService>();

            builder.Services.AddScoped<IProgressRepository, ProgressRepository>();
            builder.Services.AddScoped<IProgressService, ProgressService>();
            
            builder.Services.AddScoped<ICreditsService, CreditsService>();
            builder.Services.AddScoped<ICreditsRepository, CreditsRepository>();

            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

            builder.Services.AddScoped<IEarningService, EarningService>();
            builder.Services.AddScoped<IEarningRepository, EarningRepository>();

            builder.Services.AddScoped<IPayoutService, PayoutService>();
            builder.Services.AddScoped<IPayoutRepository, PayoutRepository>();

            builder.Services.AddScoped<ICertificateService, CertificateService>();

			builder.Services.AddTransient<IEmailSender, EmailSender>();
			builder.Services.AddTransient<ISendEmail, SendEmail>();

            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
            builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

			builder.Services.AddDistributedMemoryCache();

            builder.Services.AddScoped<IProfileService, ProfileService>();
            
            builder.Services.AddDistributedMemoryCache();


            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                    config.GetSection("Authentication:Google");
                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                });

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "YourAppSessionName";
                options.IdleTimeout = TimeSpan.FromDays(3);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //set cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Cursus";
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.ExpireTimeSpan = TimeSpan.FromDays(14); // Th?i gian h?t h?n cookie khi nh? ??ng nh?p
            });

            //set cookie


            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            builder.Services.AddSingleton(mapperConfig.CreateMapper());

            builder.Services.AddRazorPages();
            builder.Services.AddTransient<Cursus.MVC.Service.EmailSender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();           
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}
