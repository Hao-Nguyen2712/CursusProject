using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.Instructor;
using Cursus.Domain.Models;
using Cursus.Domain.ViewModels;

namespace Cursus.Infrastructure.Instructor
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly CursusDBContext dBContext;
        public InstructorRepository(CursusDBContext _dBContext)
        {
            this.dBContext = _dBContext;
        }
        public List<Account> GetInstructors()
        {
            var list = dBContext.Accounts.Where(a => a.Role == 2).ToList();
            return list;
        }

        public ProFileView GetProfile(int id, string accountid)
        {
            throw new NotImplementedException();
        }

        public bool RemoveInstructor(int id)
        {
            var student = dBContext.Accounts.FirstOrDefault(a => a.AccountId == id);
            if (student != null)
            {
                student.IsDelete = "true";
                dBContext.Accounts.Update(student);
                dBContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}