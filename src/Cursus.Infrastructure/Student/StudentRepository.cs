using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.Student;
using Cursus.Domain.Models;

namespace Cursus.Infrastructure.Student
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CursusDBContext context;
        public StudentRepository(CursusDBContext _context)
        {
            this.context = _context;
        }
        public List<Account> GetStudent()
        {
            var students = context.Accounts.Where(a => a.Role == 3).ToList();
            return students;
        }

        public bool RemoveStudent(int id)
        {
            var student = context.Accounts.FirstOrDefault(a => a.AccountId == id);
            if (student != null)
            {
                student.IsDelete = "true";
                context.Accounts.Update(student);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}