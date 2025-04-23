
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cursus.Application.Enroll;
using Cursus.Application.Account;
using Cursus.Domain.Models;
using Cursus.Domain.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Cursus.Application;


namespace Cursus.Infrastructure.Enroll
{
    public class EnrollRepository : IEnrollRepository
    {

        private readonly CursusDBContext _db;

        public EnrollRepository(CursusDBContext dbContext)
        {
            _db = dbContext;
        }

        public List<Domain.Models.Enroll> GetEnrollByAccountID(int accountID)
        {
            return _db.Enrolls.Where(x => x.AccountId == accountID).ToList();
        }

        public Domain.Models.Enroll GetEnrollByAccountId(int id)
        {
            try
            {
                // Tìm Enroll dựa trên AccountId
                var enroll = _db.Enrolls.FirstOrDefault(a => a.AccountId == id);
                return enroll;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        public Domain.Models.Enroll GetEnrollById(int id)
        {
            try
            {
                // get enroll by id
                Domain.Models.Enroll enroll = _db.Enrolls.Find(id);
                return enroll;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<Domain.Models.Enroll> GetEnrolls()
        {
            var enrolls = _db.Enrolls.ToList();
            return enrolls;
        }

        public Domain.Models.Enroll GetEnrollByEnrollId(int id)
        {
            Domain.Models.Enroll Enroll = _db.Enrolls.Where(c => c.EnrollId == id).FirstOrDefault();
            return Enroll;
        }

        public List<Domain.Models.Enroll> GetAllEnroll()
        {
            return _db.Enrolls.ToList();
        }

        public List<Domain.Models.Enroll> GetAllEnrollById(int accountId)
        {
            throw new NotImplementedException();
        }

        public List<Domain.Models.Enroll> GetAllEnrollsByCourseId(int courseId)
        {
            return _db.Enrolls
           .Where(e => e.CourseId == courseId)
           .ToList();
        }

        public List<Domain.Models.Enroll> GetAllEnrollsById(int accountId)
        {
            return _db.Enrolls
          .Where(e => e.AccountId == accountId)
          .ToList();
        }

        public Domain.Models.Enroll GetAvailableEnroll(int courseId, int accountId)
        {
            return _db.Enrolls
          .Where(e => e.CourseId == courseId && e.AccountId == accountId)
          .FirstOrDefault();
        }

        public Domain.Models.Enroll UpdateFinishByAccountID(int accountID, int courseID)
        {
            Domain.Models.Enroll enroll = _db.Enrolls.Where(e => e.AccountId == accountID && e.CourseId == courseID).FirstOrDefault();
            if (enroll != null)
            {
                enroll.EnrollFinish = DateTime.Now.ToString("yyyy-mm-dd");
                enroll.EnrollStatus = "completed";
                _db.SaveChanges();
            }
            // send mail
            return enroll;
        }
        public List<Domain.Models.Course> GetEnrolledCoursesByAccountId(int accountId)
        {
             var enrolls = _db.Enrolls.Where(e => e.AccountId == accountId).ToList();

            var enrollCourseIds = enrolls.Select(e => e.CourseId).ToList();
            var enrollCourses = _db.Courses.Where(c => enrollCourseIds.Contains(c.CourseId)).ToList();
            return enrollCourses;
        }

        public HomePageView GetData(int accountId, string id)
        {
            var enrolls = _db.Enrolls.Where(e => e.AccountId == accountId).ToList();
            var enrolledCourses = enrolls.Where(e => e.EnrollStatus == "Enrolled").ToList();
            var enrollCourseIds = enrolls.Select(e => e.CourseId).ToList();
            var enrolledCourseIds = enrolledCourses.Select(e => e.CourseId).ToList();
            var enrollCourses = _db.Courses.Where(c => enrollCourseIds.Contains(c.CourseId)).ToList();
            var enrolledCourseList = _db.Courses.Where(c => enrolledCourseIds.Contains(c.CourseId)).ToList();
            return new HomePageView
            {
                EnrolledCourse = enrolledCourseList,
                EnrollCourse = enrollCourses
            };

            
        }

        public void EnrollCourse(int courseId, int accountId)
        {
            var existingEnroll = _db.Enrolls.FirstOrDefault(e => e.CourseId == courseId && e.AccountId == accountId);
            if (existingEnroll != null)
            {
                existingEnroll.EnrollStatus = "Enrolled";
                existingEnroll.EnrollDate = DateTime.Now;
                existingEnroll.EnrollBlock = "false";
                existingEnroll.EnrollFinish = "Not finish";
                _db.Enrolls.Update(existingEnroll);
                _db.SaveChanges();
            }
        }
        public void EnrollCourseFree(int courseId, int accountId)
        {
            var existingEnroll = _db.Enrolls.FirstOrDefault(e => e.CourseId == courseId && e.AccountId == accountId);
            if (existingEnroll == null)
            {
                // If no existing enrollment, create a new one
                var newEnroll = new Domain.Models.Enroll
                {
                    CourseId = courseId,
                    AccountId = accountId,
                    EnrollBlock = "false",
                    EnrollStatus = "Enrolled",
                    EnrollFinish = "Not finish",
                    EnrollDate = DateTime.Now
                };
                _db.Enrolls.Add(newEnroll);
            }
            else
            {
                // If enrollment exists, update the status and date
                existingEnroll.EnrollStatus = "Enrolled";
                existingEnroll.EnrollDate = DateTime.Now;
                _db.Enrolls.Update(existingEnroll);
            }
            _db.SaveChanges();
        }

        public Domain.Models.Enroll GetEnrollByAccoutIDAndCourseID(int accountID, int courseID)
        {
            return _db.Enrolls.FirstOrDefault(e => e.CourseId == courseID && e.AccountId == accountID);
        }

		public bool UnEnroll(int courseId, int accountId)
		{
			var enroll = _db.Enrolls.FirstOrDefault(e => e.CourseId == courseId && e.AccountId == accountId);

			if (enroll == null)
			{
				return false;
			}

			enroll.EnrollStatus = "Purchased";
			enroll.EnrollFinish = "Not finish";
			_db.Enrolls.Update(enroll);
			_db.SaveChanges();
			return true;
		}
		public string GetEnrollStatus(int courseId, int accountId)
		{
			var enroll = _db.Enrolls
				.FirstOrDefault(e => e.CourseId == courseId && e.AccountId == accountId);

			if (enroll == null)
			{
				return "Null"; 
			}

			return enroll.EnrollStatus;
		}

		public bool DeleteProgressByCourseId(int courseId)
		{
			try
			{
				var lessonIds = _db.Lessons
					.Where(l => l.CourseId == courseId)
					.Select(l => l.LessionId)
					.ToList();

				var progressRecords = _db.Progresses
					.Where(p => lessonIds.Contains(p.LessonId))
					.ToList();

				if (progressRecords.Count > 0)
				{
					_db.Progresses.RemoveRange(progressRecords);
					_db.SaveChanges();
				}

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

        public Domain.Models.Enroll getEnrollByStatus(int courseId, int accountID) {
              var enroll =  _db.Enrolls
          .Where(e => e.CourseId == courseId && e.AccountId == accountID)
          .FirstOrDefault();
          return enroll;
        }
	}
}
