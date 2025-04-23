using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application
{
    public interface ILessonRepository
    {
        public Lesson AddLesson(Lesson lession);

        public Lesson GetLesson(int id);

		public List<Lesson> GetLessonsByCourseID(int courseID);
        public Lesson UpadateLesson(Lesson lesson);
        public int? GetLessonID(int courseId);
        public int getLessonIDEsc(int courseID);
    }
}