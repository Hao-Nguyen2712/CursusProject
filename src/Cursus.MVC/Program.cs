using AutoMapper;
using CloudinaryDotNet;
using Cursus.Application;
using Cursus.Application.Account;
using Cursus.Application.Admin;
using Cursus.Application.AdminDashBoard;
using Cursus.Application.Analyze;
using Cursus.Application.Cart;
using Cursus.Application.Category;
using Cursus.Application.Certificate;
using Cursus.Application.Comment;
using Cursus.Application.Credits;
using Cursus.Application.DashBoard;
using Cursus.Application.Earning;
using Cursus.Application.Enroll;
using Cursus.Application.Instructor;
using Cursus.Application.Payout;
using Cursus.Application.Profile;
using Cursus.Application.Progress;
using Cursus.Application.Report;
using Cursus.Application.Review;
using Cursus.Application.SearchInstructor;
using Cursus.Application.Student;
using Cursus.Application.Subscription;
using Cursus.Application.Subscrise;
using Cursus.Domain.Models;
using Cursus.Infrastructure;
using Cursus.Infrastructure.Admin;
using Cursus.Infrastructure.Analyze;
using Cursus.Infrastructure.Cart;
using Cursus.Infrastructure.Category;
using Cursus.Infrastructure.Comment;
using Cursus.Infrastructure.Credits;
using Cursus.Infrastructure.Earning;
using Cursus.Infrastructure.Enroll;
using Cursus.Infrastructure.Instructor;
using Cursus.Infrastructure.Payout;
using Cursus.Infrastructure.Progress;
using Cursus.Infrastructure.Report;
using Cursus.Infrastructure.Review;
using Cursus.Infrastructure.SearchInstructor;
using Cursus.Infrastructure.Student;
using Cursus.Infrastructure.Subscription;
using Cursus.Infrastructure.Subscrise;
using Cursus.MVC.Mapper;
using Cursus.MVC.Services;
using Cursus.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace Cursus.MVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("CursusMVCContextConnection") ?? throw new InvalidOperationException("Connection string 'CursusMVCContextConnection' not found.");
            var config = builder.Configuration;

            // Configure CursusDBContext as the main context (includes both domain models and Identity)
            builder.Services.AddDbContext<CursusDBContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Cursus.MVC")));

            // Set up Identity with ApplicationUser using CursusDBContext
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CursusDBContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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

            // Configure Cloudinary
            builder.Services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var account = new CloudinaryDotNet.Account(
                    configuration["Cloudinary:CloudName"],
                    configuration["Cloudinary:ApiKey"],
                    configuration["Cloudinary:ApiSecret"]
                );
                return new Cloudinary(account);
            });

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

            // Configure VNPay and ExchangeRate settings
            builder.Services.Configure<Cursus.Application.Models.VnPayConfig>(
                builder.Configuration.GetSection("VnPay"));
            builder.Services.Configure<Cursus.Application.Models.ExchangeRateConfig>(
                builder.Configuration.GetSection("ExchangeRate"));

            builder.Services.AddScoped<ICreditsService, CreditsService>();
            builder.Services.AddScoped<ICreditsRepository, CreditsRepository>();

            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

            builder.Services.AddScoped<IEarningService, EarningService>();
            builder.Services.AddScoped<IEarningRepository, EarningRepository>();

            builder.Services.AddScoped<IPayoutService, PayoutService>();
            builder.Services.AddScoped<IPayoutRepository, PayoutRepository>();

            builder.Services.AddScoped<ICertificateService, CertificateService>();

            // Configure Email settings
            builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("Email"));
            
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
                options.ExpireTimeSpan = TimeSpan.FromDays(14); // Cookie expiration time
            });

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            builder.Services.AddSingleton(mapperConfig.CreateMapper());

            builder.Services.AddRazorPages();

            // Register the database seeder
            builder.Services.AddScoped<DatabaseSeeder>();

            var app = builder.Build();

            // Apply migrations and seed data on startup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Migrate CursusDBContext (includes both domain models and Identity)
                    var cursusDbContext = services.GetRequiredService<CursusDBContext>();
                    cursusDbContext.Database.Migrate();

                    // Seed data if in development environment
                    if (app.Environment.IsDevelopment())
                    {
                        var seeder = services.GetRequiredService<DatabaseSeeder>();
                        await seeder.SeedAsync();
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                }
            }

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

