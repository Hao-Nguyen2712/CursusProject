using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application.SearchInstructor
{
    public interface ISearchInstructorService
    {
            Task<IEnumerable<Domain.Models.Account>> SearchInstructorsAsync(string instructorName);
    }
}