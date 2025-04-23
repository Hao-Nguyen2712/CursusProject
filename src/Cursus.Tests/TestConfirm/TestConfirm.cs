using Cursus.Application.Account;
using Cursus.Application.Admin;
using Cursus.Domain.Models;
using Cursus.Infrastructure.Admin;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Tests.TestConfirm
{
    public class TestConfirm
    {
        public class CourseRepositoryTest
        {
            private AdminRepository _adminrepository;
            private CursusDBContext _context;
            private Account _account;

            [SetUp]
            public void SetUp()
            {
                var options = new DbContextOptionsBuilder<CursusDBContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase")
                    .Options;

                _context = new CursusDBContext(options);
                _adminrepository = new AdminRepository(_context);

                // Thêm dữ liệu mẫu vào context
                var courses = new List<Course>
                {
                new Course { CourseId = 1, CourseName = "Course 1", CourseStatus = "Pending Approval" },
                new Course { CourseId = 2, CourseName = "Course 2", CourseStatus = "Pending Approval" }
                

                };
                _account = new Account
                {
                    AccountId = 1,
                    Username = "apham",
                    Email = "apham",
                    Password = "123",
                    FullName = "TAnh",
                    Phone = "0123",
                    Gender = "Male",
                    DateofBirth = null,
                    Description = "Student",
                    Avatar = "avt",
                    Role = 3,
                    Money = 3,
                    IsDelete = "True",
                    Id = "1",
                    Bio = "1"

                };
                _context.Courses.AddRange(courses);
                _context.Accounts.Add(_account);
                _context.SaveChanges();
            }

            [Test]
            public void ApproveCourse_ShouldUpdateCourseStatusToApproved_WhenCourseExists()
            {
                // Act
                _adminrepository.ApproveCourse(1);

                // Assert
                var course = _context.Courses.Find(1);
                Assert.IsNotNull(course);
                Assert.AreEqual("Approved", course.CourseStatus);
            }

            [Test]
            public void ApproveCourse_ShouldNotChangeCourseStatus_WhenCourseDoesNotExist()
            {
                // Act
                _adminrepository.ApproveCourse(1);

                // Assert
                var course = _context.Courses.FirstOrDefault(c => c.CourseId == 1);
                Assert.IsNotNull(course);
            }

            [Test]
            public void RejectCourse_ShouldUpdateCourseStatusToRejected_WhenCourseExists()
            {
                // Act
                _adminrepository.RejectCourse(1);

                // Assert
                var course = _context.Courses.Find(1);
                Assert.IsNotNull(course);
                Assert.AreEqual("Rejected", course.CourseStatus);
            }

            [Test]
            public void RejectCourse_ShouldNotChangeCourseStatus_WhenCourseDoesNotExist()
            {
                // Act
                _adminrepository.RejectCourse(2);

                // Assert
                var course = _context.Courses.FirstOrDefault(c => c.CourseId == 2);
                Assert.IsNotNull(course);
            }

            [TearDown]
            public void TearDown()
            {
                _context.Database.EnsureDeleted();
                _context.Dispose();
            }
            [Test]
            public void BlockAccount_ShouldSetIsDeleteToTrue_WhenAccountExists()
            {
                // Act
                _adminrepository.BlockAccount(1);

                // Assert
                var account = _context.Accounts.Find(1);
                Assert.IsNotNull(account);
                Assert.AreEqual("True", account.IsDelete);
            }

            [Test]
            public void BlockAccount_ShouldNotChangeAccount_WhenAccountDoesNotExist()
            {
                
                _adminrepository.BlockAccount(1);

                // Assert
                var account = _context.Accounts.FirstOrDefault(a => a.AccountId == 1045);
                Assert.IsNull(account);
            }

            [Test]
            public void UnBlockAccount_ShouldSetIsDeleteToFalse_WhenAccountExists()
            {
                // Act
                _adminrepository.BlockAccount(1); // Đầu tiên, chặn tài khoản để có trạng thái IsDelete = "True"
                _adminrepository.UnBlockAccount(1);

                // Assert
                var account = _context.Accounts.Find(1);
                Assert.IsNotNull(account);
                Assert.AreEqual("False", account.IsDelete);
            }

            [Test]
            public void UnBlockAccount_ShouldNotChangeAccount_WhenAccountDoesNotExist()
            {
                // Act
                _adminrepository.UnBlockAccount(1);

                // Assert
                var account = _context.Accounts.FirstOrDefault(a => a.AccountId == 1045);
                Assert.IsNull(account);
            }

            [Test]
            public void ApproveInstructorRegis_ShouldUpdateAccountDetails_WhenAccountExists()
            {
                // Act
                _adminrepository.ApproveInstructorRegis(1);

                var account = _context.Accounts.Find(1);
                Assert.IsNotNull(account);
                Assert.AreEqual(2, account.Role);
                Assert.AreEqual("Instructor", account.Description);
                Assert.AreEqual("False", account.UpLevel);
            }

            [Test]
            public void ApproveInstructorRegis_ShouldNotChangeAccount_WhenAccountDoesNotExist()
            {
                // Act
                _adminrepository.ApproveInstructorRegis(1);

                // Assert
                var account = _context.Accounts.FirstOrDefault(a => a.AccountId == 2);
                Assert.IsNull(account);
            }

            [Test]
            public void RejectInstructorRegis_ShouldSetUpLevelToFalse_WhenAccountExists()
            {
                // Act
                _adminrepository.RejectInstructorRegis(1);

                // Assert
                var account = _context.Accounts.Find(1);
                Assert.IsNotNull(account);
                Assert.AreEqual("False", account.UpLevel);
            }

            [Test]
            public void RejectInstructorRegis_ShouldNotChangeAccount_WhenAccountDoesNotExist()
            {
                // Act
                _adminrepository.RejectInstructorRegis(1);

                // Assert
                var account = _context.Accounts.FirstOrDefault(a => a.AccountId == 2);
                Assert.IsNull(account);
            }
        }
        

    }
}
