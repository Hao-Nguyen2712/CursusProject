using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application
{
    public interface ILessonService
    {
        public Lesson GetLesson(int id);
        public Lesson AddLesson(Lesson lession);
        public List<Lesson> GetLessonsByCourseID(int courseID);
        public Lesson UpadateLesson(Lesson lesson);

        public int? GetLessonID(int courseID);   
        public int getLessonIDEsc(int courseID);
    }
}