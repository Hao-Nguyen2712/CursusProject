using Cursus.Application;
using Cursus.Domain.Models;


namespace Cursus.Infrastructure
{
    public class LessonRepository : ILessonRepository
    {
        private readonly CursusDBContext _db;

        public LessonRepository(CursusDBContext dbContext)
        {
            _db = dbContext;
        }
        public Lesson AddLesson(Lesson lession)
        {
            // check lession is null
            if (lession == null)
            {
                return null;
            }

            // add lession to database
            _db.Lessons.Add(lession);
            _db.SaveChanges();
            return lession;
        }

        public Lesson GetLesson(int id)
        {
            // get lession by id
            Lesson lession = _db.Lessons.Find(id);
            return lession;
        }

        public List<Lesson> GetLessonsByCourseID(int courseID)
        {
            List<Lesson> lessons = _db.Lessons.Where(l => l.CourseId == courseID).ToList();
            return lessons;
        }
        public Lesson UpadateLesson(Lesson lesson)
        {
            Lesson less = _db.Lessons.Find(lesson.LessionId);
            if (less != null)
            {
                less.LessionComments = lesson.LessionComments;
                less.LessionContent = lesson.LessionContent;
                less.LessionVideo = lesson.LessionVideo;
                less.LessionTilte = lesson.LessionTilte;
                _db.Lessons.Update(less);
                _db.SaveChanges();
            }
            return less;
        }
        public int? GetLessonID(int courseID)
        {
            var lessonID = _db.Lessons.Where(l => l.CourseId == courseID)
                                      .OrderBy(l => l.LessionId)
                                      .Select(l => l.LessionId)
                                      .FirstOrDefault();
            return lessonID;
        }
        public int getLessonIDEsc(int courseID)
        {
            var lessonID = _db.Lessons.Where(l => l.CourseId == courseID)
                                      .OrderBy(l => l.LessionId)
                                      .Select(l => l.LessionId)
                                      .FirstOrDefault();
            return lessonID;
        }
    }
}