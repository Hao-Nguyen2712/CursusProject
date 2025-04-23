using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application;
using Cursus.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Cursus.Application;


namespace Cursus.Infrastructure
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CursusDBContext _db;

        public CourseRepository(CursusDBContext context)
        {
            _db = context;
        }

        public List<Domain.Models.Course> GetCourses()
        {
            var courses = _db.Courses.ToList();
            return courses;
        }

        public Domain.Models.Course AddCourse(Domain.Models.Course course)
        {
            try
            {
                // check course is null
                if (course == null)
                {
                    return null;
                }
                // add course to database
                _db.Courses.Add(course);
                _db.SaveChanges();
                return course;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<Course> GetAllCourseById(int id)
        {
            return _db.Courses
            .Where(e => e.AccountId == id)
            .ToList();
        }

        public Course GetCourseById(int id)
        {
            return _db.Courses.Find(id);
        }

        public List<Course> GetCourseByIdAndStatus(int id, string st)
        {
            return _db.Courses
            .Where(e => e.AccountId == id && e.CourseStatus == st)
            .ToList();
        }

        public int GetCourseLastest()
        {
            // lấy courseId mới nhất
            int courseID = _db.Courses.Max(c => c.CourseId);
            return courseID;

        }

        public IEnumerable<Domain.Models.Course> GetAllCourse()
        {
            var courses = _db.Courses.Include(c => c.Account).ToList();
            return courses;
        }

        public Domain.Models.Course GetCourseDetail(int id)
        {
            var course = _db.Courses.Include(c => c.Account).Include(c => c.Category).FirstOrDefault(c => c.CourseId == id);
            return course;
        }

        public int GetEnrolledCount(int courseId)
        {
            return _db.Enrolls.Count(e => e.CourseId == courseId);
        }

        public int countLessonByCourseID(int id)
        {
            int count = _db.Lessons.Where(l => l.CourseId == id).Count();
            return count;
        }

        public async Task<List<Domain.Models.Course>> GetCoursesById(List<int> ids)
        {
            // Lấy danh sách các khóa học theo danh sách các id
            List<Domain.Models.Course> courses = await _db.Courses.Where(c => ids.Contains(c.CourseId)).ToListAsync();
            return courses;
        }

        public List<Domain.Models.Course> GetCourseName(int id)
        {
            List<Domain.Models.Course> listCourse = _db.Courses.Where(course => course.AccountId == id).ToList();
            return listCourse;
        }

        public string getCourseName(string id)
        {
            throw new NotImplementedException();
        }

        public List<Domain.Models.Course> getCourseNameByAccountID(int id)
        {
            List<Domain.Models.Course> courses = _db.Courses.Where(course => course.AccountId == id).ToList();
            return courses;
        }

        public List<Course> GetAllPurchasedCourseByAccountId(int accountId)
        {
            return _db.Enrolls
              .Where(enroll => enroll.AccountId == accountId)
              .Include(enroll => enroll.Course)
              .ThenInclude(course => course.Account) // Ensure Account is included
              .Include(enroll => enroll.Course)
              .ThenInclude(course => course.Category) // Ensure Category is included
              .Select(enroll => enroll.Course)
              .ToList();
        }

        public Course UpdateCourseStatus(int id)
        {
            var course = _db.Courses.Find(id);
            if (course != null)
            {
                course.CourseStatus = "Pending Approval";
                _db.SaveChanges();
            }
            return course;
        }

        public Course DeleteCourse(int id)
        {
            var course = _db.Courses.Find(id);
            if (course != null)
            {
                course.CourseStatus = "Deleted";
                _db.SaveChanges();
            }
            return course;
        }
        public int GetCourseIDByAccountID(int accountID, int enrollID)
        {
            var courseID = _db.Enrolls
                       .Where(e => e.AccountId == accountID && e.EnrollId == enrollID)
                       .Select(e => e.CourseId.Value)
                       .FirstOrDefault();
            return courseID;
        }

        public string GetCourseNameByCourseID(int courseID)
        {
            var courseName = _db.Courses
                     .Where(c => c.CourseId == courseID)
                     .Select(c => c.CourseName)
                     .FirstOrDefault();
            return courseName;
        }
        public async Task<IEnumerable<Course>> GetCourseByNameAsync(string courseName)
        {
            IQueryable<Course> query = _db.Courses.Where(c => c.CourseStatus == "Approved");
            if (!string.IsNullOrEmpty(courseName))
            {
                query = query.Where(a => a.CourseName.ToLower().Contains(courseName.Trim().ToLower()));
            }
            return await query.ToListAsync();
        }
        public List<Domain.Models.CourseTop> GetTopCourse(int top)
        {
            var result = _db.Enrolls
                .GroupBy(c => c.CourseId)
                .Select(g => new CourseTop
                {
                    CourseID = g.Key.Value,
                    CourseCount = g.Count()
                })
                .OrderByDescending(x => x.CourseCount)
                .Take(top)
                .ToList();

            return result;
        }

        public int GetCourseCountByAccountID(int accountID)
        {
            var reult = _db.Courses
                .Where(c => c.AccountId == accountID)
                .Count();

            return reult;
        }
        public Domain.Models.Course UpdateCourseEdit(Domain.Models.Course course) {
            Domain.Models.Course courseModel = _db.Courses.Find(course.CourseId);
            if(courseModel == null) {
                return null;
            }
            courseModel.CourseName = course.CourseName;
            courseModel.CourseShortDes = course.CourseShortDes;
            courseModel.CourseDescription = course.CourseDescription;
            courseModel.CourseWlearn = course.CourseWlearn;
            courseModel.CourseRequirement = course.CourseRequirement;
            if(!course.CourseAvatar.Equals("thumbnail-demo.jpg")) {
                courseModel.CourseAvatar = "https://cursusstorageaccountv2.blob.core.windows.net/images/" + course.CourseAvatar;
            }
            
            courseModel.CourseAvatar = course.CourseAvatar;
            courseModel.CourseMoney = course.CourseMoney;
            courseModel.CourseProcess = course.CourseProcess;
            courseModel.Discount = course.Discount;
            courseModel.CategoryId = course.CategoryId; // 12
            _db.Courses.Update(courseModel);
            _db.SaveChanges();
            return courseModel;
        }

        public Domain.Models.Course GetCourseByCourseID(int courseID) {
            Domain.Models.Course course = _db.Courses.Where(c => c.CourseId == courseID).FirstOrDefault();
            return course;
        }

        public int GetSubByAccountID(string accountID)
        {
            var result = _db.Subscribes
            .Where(c => c.InstructorId == accountID).Count();
            return result;
        }


    }

}
