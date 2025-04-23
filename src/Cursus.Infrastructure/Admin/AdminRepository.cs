using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Cursus.Application.Admin;
using Cursus.Domain.Models;
namespace Cursus.Infrastructure.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private readonly CursusDBContext _db;
        public AdminRepository(CursusDBContext db)
        {
            this._db = db;
        }
        public void ApproveCourse(int courseId)
        {
            Domain.Models.Course course = _db.Courses.Find(courseId);
            course.CourseStatus = "Approved";
            _db.Courses.Update(course);
            
            _db.SaveChanges();
        }

        public void RejectCourse(int courseId)
        {
            Domain.Models.Course course = _db.Courses.Find(courseId);
            course.CourseStatus = "Rejected";
            _db.Courses.Update(course);
            _db.SaveChanges();
        }

        public List<Domain.Models.Course> getCourseApprove()
        {
            List<Domain.Models.Course> courses = _db.Courses.Where(p => p.CourseStatus == "Pending Approval").ToList();
            return courses;
        }

        public void BlockAccount(int accountId)
        {
            Domain.Models.Account account = _db.Accounts.Find(accountId);
           account.IsDelete = "True";
            _db.Accounts.Update(account);
            //SendEmailAsync(account.FullName, account.Email);
            _db.SaveChanges();
        }


        public void UnBlockAccount(int accountId)
        {
            Domain.Models.Account account = _db.Accounts.Find(accountId);
            account.IsDelete = "False";
            _db.Accounts.Update(account);
            _db.SaveChanges();
        }

        public List<Domain.Models.Account> getListAccount()
        {
            List<Domain.Models.Account> accounts = _db.Accounts.ToList();
            return accounts;
        }

        public void ApproveInstructorRegis(int accountId)
        {
            Domain.Models.Account account = _db.Accounts.Find(accountId);
            account.Role = 2;
            account.Description = "Instructor";
            account.UpLevel = "False";
            _db.Accounts.Update(account);
            _db.SaveChanges();
        }

        public void RejectInstructorRegis(int accountId)
        {
            Domain.Models.Account account = _db.Accounts.Find(accountId);
            account.UpLevel = "False";
            _db.Accounts.Update(account);
            _db.SaveChanges();
        }
        public List<Domain.Models.Account> getListInstructorRegis()
        {
            List<Domain.Models.Account> accounts = _db.Accounts.Where(p => p.Role == 3 && p.UpLevel == "True").ToList();
            return accounts;
        }

    }
}