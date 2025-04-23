using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain;
using Cursus.Domain.Models;

namespace Cursus.Application
{
    public interface ICourseRepository
    {
        IEnumerable<Domain.Models.Course> GetAllCourse();
        Domain.Models.Course GetCourseDetail(int id);
        int GetEnrolledCount(int courseId);
        public Domain.Models.Course GetCourseById(int id);
        public Domain.Models.Course AddCourse(Domain.Models.Course course);
        public int GetCourseLastest();


        public List<Cursus.Domain.Models.Course> GetCourses();
        public Task<List<Domain.Models.Course>> GetCoursesById(List<int> ids);
        public List<Domain.Models.Course> getCourseNameByAccountID(int id);
        /*public Domain.Models.Course getCourseNameByAccountIDs(int id);*/
        public int GetCourseIDByAccountID(int accountID, int enrollID);
        public string GetCourseNameByCourseID(int courseID);
        List<Course> GetAllCourseById(int id);
        List<Course> GetCourseByIdAndStatus(int id, string st);
        public int countLessonByCourseID(int id);
        List<Course> GetAllPurchasedCourseByAccountId(int accountId);
        Task<IEnumerable<Domain.Models.Course>> GetCourseByNameAsync(string courseName);
        public Course UpdateCourseStatus(int id);
        public Course DeleteCourse(int id);
        public List<Domain.Models.CourseTop> GetTopCourse(int top);
        public int GetCourseCountByAccountID(int accountID);
        public Domain.Models.Course UpdateCourseEdit(Domain.Models.Course course);
        public Domain.Models.Course GetCourseByCourseID(int courseID);
        public int GetSubByAccountID(string accountID);
    }
}