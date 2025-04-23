using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cursus.Application.Enroll;
using Cursus.Application.Subscrise;
using Cursus.Domain.Models;

namespace Cursus.Application.DashBoard
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IEnrollRepository _enrollRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ISubscriseRepository _subscriseRepository;
        public DashBoardService(IEnrollRepository enrollRepository, ICourseRepository courseRepository,ISubscriseRepository subscriseRepository)
        {
            _enrollRepository = enrollRepository;
            _courseRepository = courseRepository;
            _subscriseRepository = subscriseRepository;
        }
        public Domain.ViewModels.DashBoard GetData(int accountId,string id)
        {
            // Get all enrolls and courses for the given account ID
            var enrolls = _enrollRepository.GetAllEnroll().Where(e => e.AccountId == accountId);
            var courses = _courseRepository.GetAllCourse().Where(c => c.AccountId == accountId);
            var subscrises = _subscriseRepository.GetSubById(id);
            // Get the distinct course IDs from the enrollments
            var courseIds = courses.Select(e => e.CourseId);

            // Get the total revenue for each course ID
            var totalRevenue = courseIds.Sum(courseId =>
            {
                // Get all enrollments for the course
                var courseEnrollments = _enrollRepository.GetAllEnrollsByCourseId(courseId);

                // Get the course money for the course
                var courseMoney = _courseRepository.GetCourseById(courseId).CourseMoney;

                // Return the total revenue for the course
                return courseEnrollments.Count() * courseMoney;
            });
            var totalEnrolls = courseIds.Sum(courseId =>
            {
                // Get all enrollments for the course
                var courseEnrollments = _enrollRepository.GetAllEnrollsByCourseId(courseId);


                // Return the total revenue for the course
                return courseEnrollments.Count();
            });
            var totalEnrolledUsers = courseIds.Sum(courseId =>
            {
                // Get all enrollments for the course
                var courseEnrollments = _enrollRepository.GetAllEnrollsByCourseId(courseId);
                if (courseEnrollments.Count() == 0)
                {
                    return 0;
                }
                // Return the total revenue for the course
                return courseEnrollments.Count() / courseEnrollments.Count();
            });
            // Calculate dashboard data
            var dashboard = new Domain.ViewModels.DashBoard()
            {
                TotalCourses = courses.Count(),
                TotalRevenue = (decimal)totalRevenue,
                TotalEnrolledUsers = totalEnrolledUsers,
                TotalEnrolls = totalEnrolls,
                CourseList = _courseRepository.GetCourseByIdAndStatus(accountId,"Approved"),
                PendingCourse = _courseRepository.GetCourseByIdAndStatus(accountId,"Pending Approval"),
                RejectCourse = _courseRepository.GetCourseByIdAndStatus(accountId,"Rejected"),
                TotalSubscrise = subscrises.Count()
            };

            return dashboard;
        }
    }
}