using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application.Student
{
    public interface IStudentService
    {
        public List<Domain.Models.Account> GetStudent();
        public bool RemoveStudent(int id);
    }
}