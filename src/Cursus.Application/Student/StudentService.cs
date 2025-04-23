using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Cursus.Application.Student
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;
        public StudentService(IStudentRepository _studentRepository)
        {
            this.studentRepository = _studentRepository;
        }

        public List<Domain.Models.Account> GetStudent()
        {
            return studentRepository.GetStudent();
        }

        public bool RemoveStudent(int id)
        {
            return studentRepository.RemoveStudent(id);
        }
    }
}