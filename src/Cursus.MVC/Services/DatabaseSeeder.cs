using Cursus.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Cursus.MVC.Services
{
    public class DatabaseSeeder
    {
        private readonly CursusDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(
            CursusDBContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<DatabaseSeeder> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                await SeedCoursesAsync();
                await _context.SaveChangesAsync();
                // Check if data already exists
                if (_context.Categories.Any())
                {
                    _logger.LogInformation("Database already seeded.");
                    return;
                }

                _logger.LogInformation("Starting database seeding...");

                // Create roles first
                await CreateRolesAsync();

                // Seed database tables in order (save each group separately to get generated IDs)
                await SeedCategoriesAsync();
                await _context.SaveChangesAsync(); // Save categories first to get IDs

                // Create Identity users and accounts together
                await CreateIdentityUsersAndAccountsAsync();
                await _context.SaveChangesAsync(); // Save accounts to get IDs

                await SeedCoursesAsync();
                await _context.SaveChangesAsync(); // Save courses to get IDs

                await SeedLessonsAsync();
                await _context.SaveChangesAsync(); // Save lessons to get IDs

                // Ensure courses are seeded before enrollments
                await SeedEnrollmentsAsync();
                await SeedCartsAsync();
                await SeedRatingsAsync();
                await SeedProgressAsync();
                await SeedCommentsAsync();
                await SeedTradingsAsync();

                await _context.SaveChangesAsync(); // Final save for remaining data
                _logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task CreateRolesAsync()
        {
            string[] roles = { "Admin", "Instructor", "Student" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task CreateIdentityUsersAndAccountsAsync()
        {
            var usersData = new[]
            {
                new {
                    Id = "admin-user-1", Email = "admin@cursus.com", Role = "Admin",
                    Username = "admin", FullName = "System Administrator", Phone = "+1234567890",
                    Gender = "Male", DateofBirth = new DateTime(1985, 1, 15),
                    Description = "System Administrator Account", Avatar = "/images/avatars/admin.jpg",
                    UserRole = 1, Money = 0m, UpLevel = "Admin",
                    Bio = "Experienced system administrator managing the Cursus platform."
                },
                new {
                    Id = "instructor-user-2", Email = "instructor1@cursus.com", Role = "Instructor",
                    Username = "johnsmith", FullName = "John Smith", Phone = "+1234567891",
                    Gender = "Male", DateofBirth = new DateTime(1988, 3, 22),
                    Description = "Senior Web Development Instructor", Avatar = "/images/avatars/john.jpg",
                    UserRole = 2, Money = 15000.00m, UpLevel = "Senior",
                    Bio = "Full-stack developer with 8+ years of experience in modern web technologies."
                },
                new {
                    Id = "instructor-user-3", Email = "instructor2@cursus.com", Role = "Instructor",
                    Username = "maryjohnson", FullName = "Mary Johnson", Phone = "+1234567892",
                    Gender = "Female", DateofBirth = new DateTime(1990, 7, 10),
                    Description = "Data Science and ML Instructor", Avatar = "/images/avatars/mary.jpg",
                    UserRole = 2, Money = 12500.00m, UpLevel = "Senior",
                    Bio = "Data scientist specializing in machine learning and statistical analysis."
                },
                new {
                    Id = "student-user-4", Email = "student1@cursus.com", Role = "Student",
                    Username = "mikebrown", FullName = "Mike Brown", Phone = "+1234567893",
                    Gender = "Male", DateofBirth = new DateTime(1995, 11, 5),
                    Description = "Computer Science Student", Avatar = "/images/avatars/mike.jpg",
                    UserRole = 3, Money = 500.00m, UpLevel = "Beginner",
                    Bio = "Aspiring software developer currently learning web development."
                },
                new {
                    Id = "student-user-5", Email = "student2@cursus.com", Role = "Student",
                    Username = "sarahwilson", FullName = "Sarah Wilson", Phone = "+1234567894",
                    Gender = "Female", DateofBirth = new DateTime(1997, 4, 18),
                    Description = "Marketing Professional Learning Tech", Avatar = "/images/avatars/sarah.jpg",
                    UserRole = 3, Money = 750.00m, UpLevel = "Intermediate",
                    Bio = "Marketing professional transitioning to tech through online learning."
                },
                new {
                    Id = "student-user-6", Email = "student3@cursus.com", Role = "Student",
                    Username = "davidlee", FullName = "David Lee", Phone = "+1234567895",
                    Gender = "Male", DateofBirth = new DateTime(1992, 9, 25),
                    Description = "Career Changer", Avatar = "/images/avatars/david.jpg",
                    UserRole = 3, Money = 300.00m, UpLevel = "Beginner",
                    Bio = "Former accountant learning programming to change careers."
                },
                new {
                    Id = "student-user-7", Email = "student4@cursus.com", Role = "Student",
                    Username = "emilydavis", FullName = "Emily Davis", Phone = "+1234567896",
                    Gender = "Female", DateofBirth = new DateTime(1994, 12, 8),
                    Description = "Graphic Designer Learning Development", Avatar = "/images/avatars/emily.jpg",
                    UserRole = 3, Money = 950.00m, UpLevel = "Intermediate",
                    Bio = "Creative professional expanding skills into web development."
                },
                new {
                    Id = "student-user-8", Email = "student5@cursus.com", Role = "Student",
                    Username = "alexgarcia", FullName = "Alex Garcia", Phone = "+1234567897",
                    Gender = "Male", DateofBirth = new DateTime(1996, 6, 30),
                    Description = "Recent Graduate", Avatar = "/images/avatars/alex.jpg",
                    UserRole = 3, Money = 200.00m, UpLevel = "Beginner",
                    Bio = "Recent computer science graduate looking to specialize in data science."
                },
                new {
                    Id = "instructor-user-9", Email = "instructor3@cursus.com", Role = "Instructor",
                    Username = "lisachen", FullName = "Lisa Chen", Phone = "+1234567898",
                    Gender = "Female", DateofBirth = new DateTime(1987, 2, 14),
                    Description = "Mobile Development Expert", Avatar = "/images/avatars/lisa.jpg",
                    UserRole = 2, Money = 18000.00m, UpLevel = "Expert",
                    Bio = "Mobile app developer with expertise in iOS and Android development."
                },
                new {
                    Id = "instructor-user-10", Email = "instructor4@cursus.com", Role = "Instructor",
                    Username = "robertmiller", FullName = "Robert Miller", Phone = "+1234567899",
                    Gender = "Male", DateofBirth = new DateTime(1985, 10, 3),
                    Description = "Cloud Architecture Specialist", Avatar = "/images/avatars/robert.jpg",
                    UserRole = 2, Money = 22000.00m, UpLevel = "Expert",
                    Bio = "Cloud solutions architect with extensive experience in AWS and Azure."
                }
            };

            foreach (var userData in usersData)
            {
                // Create Identity user first
                var existingUser = await _userManager.FindByEmailAsync(userData.Email);
                if (existingUser == null)
                {
                    var user = new ApplicationUser
                    {
                        Id = userData.Id,
                        UserName = userData.Email,
                        Email = userData.Email,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(user, "TempPassword123!");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, userData.Role);

                        // Create corresponding Account record
                        var account = new Account
                        {
                            Username = userData.Username,
                            Email = userData.Email,
                            Password = "hashed_password_" + userData.Username,
                            FullName = userData.FullName,
                            Phone = userData.Phone,
                            Gender = userData.Gender,
                            DateofBirth = userData.DateofBirth,
                            Description = userData.Description,
                            Avatar = userData.Avatar,
                            Role = userData.UserRole,
                            Money = userData.Money,
                            UpLevel = userData.UpLevel,
                            IsDelete = "False",
                            Id = userData.Id, // Link to AspNetUsers.Id
                            Bio = userData.Bio
                        };

                        _context.Accounts.Add(account);
                    }
                }
            }
        }

        private async Task SeedCategoriesAsync()
        {
            var categories = new[]
            {
                new Category { CategoryName = "Web Development", CategoryStatus = "Active" },
                new Category { CategoryName = "Mobile Development", CategoryStatus = "Active" },
                new Category { CategoryName = "Data Science", CategoryStatus = "Active" },
                new Category { CategoryName = "Machine Learning", CategoryStatus = "Active" },
                new Category { CategoryName = "Cloud Computing", CategoryStatus = "Active" },
                new Category { CategoryName = "DevOps", CategoryStatus = "Active" },
                new Category { CategoryName = "UI/UX Design", CategoryStatus = "Active" },
                new Category { CategoryName = "Database Management", CategoryStatus = "Active" },
                new Category { CategoryName = "Cybersecurity", CategoryStatus = "Active" },
                new Category { CategoryName = "Game Development", CategoryStatus = "Active" }
            };

            _context.Categories.AddRange(categories);
        }



        private async Task SeedCoursesAsync()
        {
            // Get the seeded accounts and categories to reference their auto-generated IDs
            var accounts = _context.Accounts.ToList();
            var categories = _context.Categories.ToList();

            if (accounts.Count >= 5 && categories.Count >= 5)
            {
                var courses = new[]
                {
                    new Course
                    {
                        AccountId = accounts[1].AccountId, CourseName = "Complete Web Development Bootcamp",
                        CourseShortDes = "Learn full-stack web development from scratch",
                        CourseDescription = "A comprehensive course covering HTML, CSS, JavaScript, React, Node.js, and database management. Perfect for beginners who want to become full-stack developers.",
                        CourseWlearn = "HTML5, CSS3, JavaScript ES6+, React, Node.js, Express, MongoDB, Git, Deployment",
                        CourseRequirement = "Basic computer skills and willingness to learn. No prior programming experience required.",
                        CourseAvatar = "/images/courses/web-development.jpg", CourseDate = new DateTime(2024, 1, 15),
                        CourseMoney = 299.99m, CourseStatus = "Approved", CourseProcess = 100, Discount = 0.15m, CategoryId = categories[0].CategoryId
                    },
                    new Course
                    {
                        AccountId = accounts[8].AccountId, CourseName = "iOS App Development with Swift",
                        CourseShortDes = "Build native iOS applications using Swift and SwiftUI",
                        CourseDescription = "Learn to create beautiful iOS applications from the ground up. Covers Swift fundamentals, UIKit, SwiftUI, Core Data, and App Store deployment.",
                        CourseWlearn = "Swift Programming, SwiftUI, UIKit, Core Data, Networking, App Store Guidelines",
                        CourseRequirement = "Mac computer with Xcode installed. Basic programming knowledge helpful but not required.",
                        CourseAvatar = "/images/courses/ios-development.jpg", CourseDate = new DateTime(2024, 2, 1),
                        CourseMoney = 349.99m, CourseStatus = "Approved", CourseProcess = 95, Discount = 0.20m, CategoryId = categories[1].CategoryId
                    },
                    new Course
                    {
                        AccountId = accounts[2].AccountId, CourseName = "Data Science and Machine Learning Masterclass",
                        CourseShortDes = "Master data analysis, visualization, and machine learning",
                        CourseDescription = "Comprehensive data science course covering Python, pandas, NumPy, scikit-learn, and TensorFlow. Learn to analyze data and build predictive models.",
                        CourseWlearn = "Python, Pandas, NumPy, Matplotlib, Seaborn, Scikit-learn, TensorFlow, Jupyter Notebooks",
                        CourseRequirement = "Basic mathematics knowledge. Python experience helpful but not required.",
                        CourseAvatar = "/images/courses/data-science.jpg", CourseDate = new DateTime(2024, 1, 20),
                        CourseMoney = 399.99m, CourseStatus = "Approved", CourseProcess = 100, Discount = 0.25m, CategoryId = categories[2].CategoryId
                    },
                    new Course
                    {
                        AccountId = accounts[9].AccountId, CourseName = "AWS Cloud Solutions Architecture",
                        CourseShortDes = "Design and deploy scalable cloud solutions on AWS",
                        CourseDescription = "Learn to architect, deploy, and manage applications on Amazon Web Services. Covers EC2, S3, RDS, Lambda, and more.",
                        CourseWlearn = "AWS Services, EC2, S3, RDS, Lambda, CloudFormation, Security Best Practices",
                        CourseRequirement = "Basic understanding of networking and server concepts. Some programming experience recommended.",
                        CourseAvatar = "/images/courses/aws-cloud.jpg", CourseDate = new DateTime(2024, 2, 10),
                        CourseMoney = 449.99m, CourseStatus = "Approved", CourseProcess = 85, Discount = 0.10m, CategoryId = categories[4].CategoryId
                    },
                    new Course
                    {
                        AccountId = accounts[1].AccountId, CourseName = "React and Redux for Modern Web Apps",
                        CourseShortDes = "Build dynamic web applications with React and Redux",
                        CourseDescription = "Deep dive into React ecosystem including hooks, context, Redux for state management, and modern development practices.",
                        CourseWlearn = "React, Redux, React Hooks, Context API, React Router, Testing with Jest",
                        CourseRequirement = "Basic JavaScript knowledge required. HTML/CSS experience recommended.",
                        CourseAvatar = "/images/courses/react-redux.jpg", CourseDate = new DateTime(2024, 3, 1),
                        CourseMoney = 249.99m, CourseStatus = "Approved", CourseProcess = 75, Discount = 0.30m, CategoryId = categories[0].CategoryId
                    }
                };

                _context.Courses.AddRange(courses);
            }
        }

        private async Task SeedLessonsAsync()
        {
            // Get the seeded courses to reference their auto-generated IDs
            var courses = _context.Courses.ToList();

            if (courses.Count >= 3)
            {
                var lessons = new[]
                {
                    new Lesson
                    {
                        CourseId = courses[0].CourseId, LessionTilte = "Introduction to HTML",
                        LessionType = "Video", LessionVideo = "/videos/lessons/html-intro.mp4",
                        LessionContent = "Learn the basics of HTML structure and elements",
                        LessionComments = 15, LessionFinish = 1, LessionImage = "/images/lessons/html-intro.jpg"
                    },
                    new Lesson
                    {
                        CourseId = courses[0].CourseId, LessionTilte = "CSS Fundamentals",
                        LessionType = "Video", LessionVideo = "/videos/lessons/css-fundamentals.mp4",
                        LessionContent = "Understanding CSS selectors, properties, and styling techniques",
                        LessionComments = 12, LessionFinish = 1, LessionImage = "/images/lessons/css-fundamentals.jpg"
                    },
                    new Lesson
                    {
                        CourseId = courses[1].CourseId, LessionTilte = "Swift Programming Basics",
                        LessionType = "Video", LessionVideo = "/videos/lessons/swift-basics.mp4",
                        LessionContent = "Introduction to Swift syntax, variables, and control structures",
                        LessionComments = 8, LessionFinish = 1, LessionImage = "/images/lessons/swift-basics.jpg"
                    },
                    new Lesson
                    {
                        CourseId = courses[1].CourseId, LessionTilte = "Building Your First iOS App",
                        LessionType = "Video", LessionVideo = "/videos/lessons/first-ios-app.mp4",
                        LessionContent = "Step-by-step guide to creating a simple iOS application",
                        LessionComments = 20, LessionFinish = 1, LessionImage = "/images/lessons/first-ios-app.jpg"
                    },
                    new Lesson
                    {
                        CourseId = courses[2].CourseId, LessionTilte = "Python for Data Science",
                        LessionType = "Video", LessionVideo = "/videos/lessons/python-data-science.mp4",
                        LessionContent = "Python basics and libraries essential for data science",
                        LessionComments = 25, LessionFinish = 1, LessionImage = "/images/lessons/python-data-science.jpg"
                    }
                };

                _context.Lessons.AddRange(lessons);
            }
        }

        private async Task SeedEnrollmentsAsync()
        {
            var courses = _context.Courses.ToList();
            var accounts = _context.Accounts.ToList();

            if (courses.Count >= 3 && accounts.Count >= 5)
            {
                var enrollments = new[]
                {
                    new Enroll
                    {
                        CourseId = courses[0].CourseId, AccountId = accounts[3].AccountId, EnrollFinish = "Active",
                        EnrollDate = new DateTime(2024, 1, 20), EnrollBlock = "No", EnrollStatus = "Active"
                    },
                    new Enroll
                    {
                        CourseId = courses[0].CourseId, AccountId = accounts[4].AccountId, EnrollFinish = "Completed",
                        EnrollDate = new DateTime(2024, 1, 18), EnrollBlock = "No", EnrollStatus = "Completed"
                    },
                    new Enroll
                    {
                        CourseId = courses[1].CourseId, AccountId = accounts[5].AccountId, EnrollFinish = "Active",
                        EnrollDate = new DateTime(2024, 2, 5), EnrollBlock = "No", EnrollStatus = "Active"
                    },
                    new Enroll
                    {
                        CourseId = courses[2].CourseId, AccountId = accounts[6].AccountId, EnrollFinish = "Active",
                        EnrollDate = new DateTime(2024, 1, 25), EnrollBlock = "No", EnrollStatus = "Active"
                    },
                    new Enroll
                    {
                        CourseId = courses[2].CourseId, AccountId = accounts[7].AccountId, EnrollFinish = "Active",
                        EnrollDate = new DateTime(2024, 2, 1), EnrollBlock = "No", EnrollStatus = "Active"
                    }
                };

                _context.Enrolls.AddRange(enrollments);
            }
        }

        private async Task SeedCartsAsync()
        {
            var courses = _context.Courses.ToList();
            var accounts = _context.Accounts.ToList();

            if (courses.Count >= 3 && accounts.Count >= 7)
            {
                var carts = new[]
                {
                    new Cart { CourseId = courses[3].CourseId, AccountId = accounts[3].AccountId, CartMoney = 449.99m },
                    new Cart { CourseId = courses[4].CourseId, AccountId = accounts[4].AccountId, CartMoney = 249.99m },
                    new Cart { CourseId = courses[1].CourseId, AccountId = accounts[6].AccountId, CartMoney = 349.99m }
                };

                _context.Carts.AddRange(carts);
            }
        }

        private async Task SeedRatingsAsync()
        {
            var courses = _context.Courses.ToList();
            var accounts = _context.Accounts.ToList();

            if (courses.Count >= 2 && accounts.Count >= 6)
            {
                var ratings = new[]
                {
                    new Rate
                    {
                        CourseId = courses[0].CourseId, AccountId = accounts[4].AccountId, RatePoint = 5,
                        RateContent = "Excellent course! Very comprehensive and well-structured. Learned a lot!",
                        RateDate = new DateTime(2024, 2, 15)
                    },
                    new Rate
                    {
                        CourseId = courses[0].CourseId, AccountId = accounts[3].AccountId, RatePoint = 4,
                        RateContent = "Great course for beginners. Could use more advanced topics.",
                        RateDate = new DateTime(2024, 2, 10)
                    },
                    new Rate
                    {
                        CourseId = courses[1].CourseId, AccountId = accounts[5].AccountId, RatePoint = 5,
                        RateContent = "Amazing instructor! Clear explanations and practical examples.",
                        RateDate = new DateTime(2024, 2, 20)
                    }
                };

                _context.Rates.AddRange(ratings);
            }
        }

        private async Task SeedProgressAsync()
        {
            var lessons = _context.Lessons.ToList();
            var accounts = _context.Accounts.ToList();

            if (lessons.Count >= 3 && accounts.Count >= 3)
            {
                var progresses = new[]
                {
                    new Progress
                    {
                        AccId = accounts[0].Id, LessonId = lessons[0].LessionId, Finish = "Completed"
                    },
                    new Progress
                    {
                        AccId = accounts[0].Id, LessonId = lessons[1].LessionId, Finish = "InProgress"
                    },
                    new Progress
                    {
                        AccId = accounts[1].Id, LessonId = lessons[0].LessionId, Finish = "Completed"
                    },
                    new Progress
                    {
                        AccId = accounts[1].Id, LessonId = lessons[1].LessionId, Finish = "Completed"
                    },
                    new Progress
                    {
                        AccId = accounts[2].Id, LessonId = lessons[2].LessionId, Finish = "Completed"
                    }
                };

                _context.Progresses.AddRange(progresses);
            }
        }

        private async Task SeedCommentsAsync()
        {
            var lessons = _context.Lessons.ToList();
            var accounts = _context.Accounts.ToList();

            if (lessons.Count >= 3 && accounts.Count >= 6)
            {
                var comments = new[]
                {
                    new Comment
                    {
                        AccountId = accounts[3].AccountId, LessionId = lessons[0].LessionId,
                        CmtContent = "Great introduction! Very clear explanation of HTML basics.",
                        CmtLevel = 1, CmtReply = null, CmtDate = new DateTime(2024, 1, 22)
                    },
                    new Comment
                    {
                        AccountId = accounts[4].AccountId, LessionId = lessons[0].LessionId,
                        CmtContent = "Thanks for the detailed examples. Really helpful!",
                        CmtLevel = 1, CmtReply = null, CmtDate = new DateTime(2024, 1, 23)
                    },
                    new Comment
                    {
                        AccountId = accounts[5].AccountId, LessionId = lessons[2].LessionId,
                        CmtContent = "Could you provide more examples on optionals?",
                        CmtLevel = 1, CmtReply = null, CmtDate = new DateTime(2024, 2, 8)
                    }
                };

                _context.Comments.AddRange(comments);
            }
        }

        private async Task SeedTradingsAsync()
        {
            var accounts = _context.Accounts.ToList();

            if (accounts.Count >= 8)
            {
                var tradings = new[]
                {
                    new Trading
                    {
                        AccountId = accounts[3].AccountId, TdMoney = 299.99m,
                        TdDate = new DateTime(2024, 1, 20), TdMethodPayment = "Credit Card"
                    },
                    new Trading
                    {
                        AccountId = accounts[4].AccountId, TdMoney = 299.99m,
                        TdDate = new DateTime(2024, 1, 18), TdMethodPayment = "PayPal"
                    },
                    new Trading
                    {
                        AccountId = accounts[5].AccountId, TdMoney = 349.99m,
                        TdDate = new DateTime(2024, 2, 5), TdMethodPayment = "Credit Card"
                    },
                    new Trading
                    {
                        AccountId = accounts[6].AccountId, TdMoney = 399.99m,
                        TdDate = new DateTime(2024, 1, 25), TdMethodPayment = "Debit Card"
                    },
                    new Trading
                    {
                        AccountId = accounts[7].AccountId, TdMoney = 399.99m,
                        TdDate = new DateTime(2024, 2, 1), TdMethodPayment = "Credit Card"
                    },
                    new Trading
                    {
                        AccountId = accounts[3].AccountId, TdMoney = 449.99m,
                        TdDate = new DateTime(2024, 2, 15), TdMethodPayment = "PayPal"
                    },
                    new Trading
                    {
                        AccountId = accounts[4].AccountId, TdMoney = 249.99m,
                        TdDate = new DateTime(2024, 3, 2), TdMethodPayment = "Credit Card"
                    },
                    new Trading
                    {
                        AccountId = accounts[5].AccountId, TdMoney = 329.99m,
                        TdDate = new DateTime(2024, 2, 20), TdMethodPayment = "Debit Card"
                    },
                    new Trading
                    {
                        AccountId = accounts[6].AccountId, TdMoney = 499.99m,
                        TdDate = new DateTime(2024, 3, 8), TdMethodPayment = "Credit Card"
                    },
                    new Trading
                    {
                        AccountId = accounts[7].AccountId, TdMoney = 379.99m,
                        TdDate = new DateTime(2024, 2, 25), TdMethodPayment = "PayPal"
                    }
                };

                _context.Tradings.AddRange(tradings);
            }
        }
    }
}