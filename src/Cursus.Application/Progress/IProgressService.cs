using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Application.Progress
{
    public interface IProgressService
    {
        public Cursus.Domain.Models.Progress AddProgress(Cursus.Domain.Models.Progress progress);
        public Cursus.Domain.Models.Progress GetProgress(int id);
        public int coutProgressByAccountID(int lessonID, string accountID);
        public Cursus.Domain.Models.Progress GetProgressByAccountIDAndLessonID(string accountID, int lessonID);
        public int DeleteProgress(string accountID, int lessonID);
        public List<int> GetListLessonIDByCourseID(int courseID);
    }
}
