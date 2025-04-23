
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cursus.Domain;
using Cursus.Domain.Models;
using Cursus.Domain.ViewModels;


namespace Cursus.Application.Enroll
{
    public interface IEnrollRepository
    {
        public List<Cursus.Domain.Models.Enroll> GetEnrolls();
        public Domain.Models.Enroll GetEnrollById(int id);
        public List<Cursus.Domain.Models.Enroll> GetEnrollByAccountID(int accountID);
        public Domain.Models.Enroll GetEnrollByAccountId(int id);
        public Domain.Models.Enroll GetEnrollByEnrollId(int id);
        List<Domain.Models.Enroll> GetAllEnroll();
        Domain.Models.Enroll GetAvailableEnroll(int courseId, int accountId);
        List<Domain.Models.Enroll> GetAllEnrollsById(int accountId);
        List<Domain.Models.Enroll> GetAllEnrollsByCourseId(int courseId);
        Domain.Models.Enroll UpdateFinishByAccountID(int accountID, int courseID);
        List<Domain.Models.Course> GetEnrolledCoursesByAccountId(int accountId);
        public void EnrollCourse(int courseId, int accountId);
        public void EnrollCourseFree(int courseId, int accountId);
        public Domain.Models.Enroll GetEnrollByAccoutIDAndCourseID(int accountID, int courseID);
        public bool UnEnroll(int courseId, int accountId);
        public string GetEnrollStatus(int courseId, int accountId);
        public bool DeleteProgressByCourseId(int courseId);
        HomePageView GetData(int accountId,string id);
        public Domain.Models.Enroll getEnrollByStatus(int courseId, int accountID);
	}
}
