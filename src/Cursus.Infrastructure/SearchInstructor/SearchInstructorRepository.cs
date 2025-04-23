using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application;
using Cursus.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Cursus.Infrastructure.SearchInstructor
{
    public class SearchInstructorRepository : ISearchInstructorRepository
    {

        private readonly CursusDBContext _db;

        public SearchInstructorRepository(CursusDBContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<IEnumerable<Domain.Models.Account>> GetInstructorsByNameAsync(string instructorName)
        {
            IQueryable<Domain.Models.Account> query = _db.Accounts.Where(a => a.Role == 2);

            if (!string.IsNullOrEmpty(instructorName))
            {
                query = query.Where(a => a.Username.ToLower().Contains(instructorName.Trim().ToLower()) || a.FullName.ToLower().Contains(instructorName.Trim().ToLower())
                || a.Email.ToLower().Contains(instructorName.Trim().ToLower()));
            }

            return await query.ToListAsync();
        }
    }
}