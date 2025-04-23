using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application
{
    public interface ISearchInstructorRepository
    {
        Task<IEnumerable<Domain.Models.Account>> GetInstructorsByNameAsync(string instructorName);
    }
}