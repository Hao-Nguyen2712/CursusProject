using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.Application;

namespace Cursus.Application
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessionRepository;

        public LessonService(ILessonRepository lessionRepository)
        {
            _lessionRepository = lessionRepository;
        }

        public Lesson? AddLesson(Lesson lession)
        {
            // check lession is null
            if (lession == null)
            {
                return null;
            }
            // add lession to database
            _lessionRepository.AddLesson(lession);
            return lession;
        }

        public Lesson GetLesson(int id)
        {
            // get lession by id
            Lesson lession = _lessionRepository.GetLesson(id);
            return lession;
        }

        public List<Lesson> GetLessonsByCourseID(int courseID)
        {
            List<Lesson> lessons = _lessionRepository.GetLessonsByCourseID(courseID);
            return lessons;
        }
        public Lesson UpadateLesson(Lesson lesson)
        {
            return _lessionRepository.UpadateLesson(lesson);
        }

        public int? GetLessonID(int courseID){
            return _lessionRepository.GetLessonID(courseID);
        }
        public int getLessonIDEsc(int courseID)
        {
            return _lessionRepository.getLessonIDEsc(courseID);
        }
    }
}