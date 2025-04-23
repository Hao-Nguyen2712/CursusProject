using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application.SearchInstructor
{

    public class SearchInstructorService : ISearchInstructorService
    {
        private readonly ISearchInstructorRepository _db;
        public SearchInstructorService(ISearchInstructorRepository db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Domain.Models.Account>> SearchInstructorsAsync(string instructorName)
        {
            return await _db.GetInstructorsByNameAsync(instructorName);
        }
    }
}