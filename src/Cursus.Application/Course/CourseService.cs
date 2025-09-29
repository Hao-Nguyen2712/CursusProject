using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application;
using Cursus.Application.Account;
using Cursus.Domain.Models;

namespace Cursus.Application
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public List<Domain.Models.Course> GetCourses()
        {
            var courseService = _courseRepository.GetCourses();
            return courseService;
        }
        public IEnumerable<Domain.Models.Course> GetAllCourse()
        {
            return _courseRepository.GetAllCourse();
        }

        public Domain.Models.Course GetCourseDetail(int id)
        {
            var courseDetail = _courseRepository.GetCourseDetail(id);
            if (courseDetail == null) return null;
            return courseDetail;
        }

        public int GetEnrolledCount(int courseId)
        {
            var countEnroll = _courseRepository.GetEnrolledCount(courseId);
            return countEnroll;
        }
        public Course AddCourse(Course course)
        {
            if (course == null)
            {
                return null;
            }
            _courseRepository.AddCourse(course);
            return course;
        }


        public List<Course> GetAllCourseById(int id)
        {
            return _courseRepository.GetAllCourseById(id);
        }

        public Course GetCourseById(int id)
        {
            return _courseRepository.GetCourseById(id);
        }

        public List<Course> GetCourseByIdAndStatus(int id, string st)
        {
            return _courseRepository.GetCourseByIdAndStatus(id, st);
        }

        public int GetCourseLastest()
        {
            int id = _courseRepository.GetCourseLastest();
            return id;
        }

        public int countLessonByCourseID(int id)
        {
            return _courseRepository.countLessonByCourseID(id);
        }

        public Task<List<Domain.Models.Course>> GetCoursesById(List<int> ids)
        {
            var courseService = _courseRepository.GetCoursesById(ids);
            return courseService;
        }

        public List<Domain.Models.Course> getCourseNameByAccountID(int id)
        {
            List<Domain.Models.Course> courses = _courseRepository.getCourseNameByAccountID(id);
            return courses;
        }

        public List<Course> GetAllPurchasedCourseByAccountId(int accountId)
        {
            return _courseRepository.GetAllPurchasedCourseByAccountId(accountId);
        }
        public Course UpdateCourseStatus(int id)
        {
            return _courseRepository.UpdateCourseStatus(id);
        }

        public Course DeleteCourse(int id)
        {
            return _courseRepository.DeleteCourse(id);
        }

        public int GetCourseIDByAccountID(int accountID, int enrollID)
        {
            return _courseRepository.GetCourseIDByAccountID(accountID, enrollID);
        }
        public string GetCourseNameByCourseID(int courseID)
        {
            return _courseRepository.GetCourseNameByCourseID(courseID);
        }
        public List<Domain.Models.CourseTop> GetTopCourse(int top)
        {
            return _courseRepository.GetTopCourse(top);
        }

        public async Task<IEnumerable<Course>> GetCourseByNameAsync(string courseName)
        {
            return await _courseRepository.GetCourseByNameAsync(courseName);
        }
        public int GetCourseCountByAccountID(int accountID)
        {
            return _courseRepository.GetCourseCountByAccountID(accountID);
         }
        public Domain.Models.Course UpdateCourseEdit(Domain.Models.Course course) {
            return _courseRepository.UpdateCourseEdit(course);
        }

        public Domain.Models.Course GetCourseByCourseID(int courseID) {
            return _courseRepository.GetCourseByCourseID(courseID);
        }

        public int GetSubByAccountID(string accountID)
        {
            return _courseRepository.GetSubByAccountID(accountID);
        }

        public List<Course> GetAllCourseActive()
        {
            return _courseRepository.GetAllCourse().Where(c => c.CourseStatus == "Active").ToList();
        }

        public List<Course> GetAllCoursePending()
        {
            return _courseRepository.GetAllCourse().Where(c => c.CourseStatus == "Pending").ToList();
        }

    }
}




