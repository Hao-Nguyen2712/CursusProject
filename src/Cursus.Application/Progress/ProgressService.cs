using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Application.Progress
{
    public class ProgressService : IProgressService
    {
        private readonly IProgressRepository _progressRepository;
        public ProgressService(IProgressRepository progressRepository)
        {
            _progressRepository = progressRepository;
        }
        public Domain.Models.Progress AddProgress(Domain.Models.Progress progress)
        {
            if (progress == null)
            {
                throw new ArgumentNullException(nameof(progress));
            }
            return _progressRepository.AddProgress(progress);
        }

        public int coutProgressByAccountID(int lessonID, string accountID)
        {
            return _progressRepository.coutProgressByAccountID(lessonID, accountID);
        }

        public int DeleteProgress(string accountID, int lessonID)
        {
             return _progressRepository.DeleteProgress(accountID, lessonID);
        }

        public Domain.Models.Progress GetProgress(int id)
        {
           return _progressRepository.GetProgress(id);
        }

        public Domain.Models.Progress GetProgressByAccountIDAndLessonID(string accountID, int lessonID)
        {
            return _progressRepository.GetProgressByAccountIDAndLessonID(accountID, lessonID);
        }
        public List<int> GetListLessonIDByCourseID(int courseID){
            return _progressRepository.GetListLessonIDByCourseID(courseID);
        }
    }
}
