using Cursus.Application.Progress;
using Cursus.Domain.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Infrastructure.Progress
{
    public class ProgressRepository : IProgressRepository
    {
        private readonly CursusDBContext _db;
        public ProgressRepository(CursusDBContext dbContext)
        {
            _db = dbContext;
        }
        public Domain.Models.Progress AddProgress(Domain.Models.Progress progress)
        {
            _db.Progresses.Add(progress);
            _db.SaveChanges();
            return progress;
        }

        public int coutProgressByAccountID(int lessonID, string accountID)
        {
            int count = _db.Progresses.Where(p => p.LessonId == lessonID && p.AccId == accountID).Count();
            return count;
        }

        public int DeleteProgress(string accountID, int lessonID)
        {
            Domain.Models.Progress progress = _db.Progresses.Where(p => p.AccId == accountID && p.LessonId == lessonID).FirstOrDefault();
            if(progress != null)
            {
                _db.Progresses.Remove(progress);
                _db.SaveChanges();
                return 1;
            }
            return 0;
        }

        public Domain.Models.Progress GetProgress(int id)
        {
            return _db.Progresses.Find(id);
        }

        public Domain.Models.Progress GetProgressByAccountIDAndLessonID(string accountID, int lessonID)
        {
            Domain.Models.Progress progress = new Domain.Models.Progress();
            progress =  _db.Progresses.Where(p => p.AccId == accountID && p.LessonId == lessonID).FirstOrDefault();
            if(progress == null)
            {
                return null;
            }
            return progress;
        }

        public List<int> GetListLessonIDByCourseID(int courseID)
        {
            List<int> listLessonID = new List<int>();
            listLessonID = _db.Lessons.Where(l => l.CourseId == courseID).Select(l => l.LessionId).ToList();
            return listLessonID;
        }
    }
}
