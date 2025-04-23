using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.Subscrise;
using Cursus.Domain.Models;

namespace Cursus.Infrastructure.Subscrise
{
    public class SubscriseRepository : ISubscriseRepository
    {
        private readonly CursusDBContext _context;
        public SubscriseRepository(CursusDBContext context)
        {
            _context = context;
        }

        public void AddSub(Subscribe sub)
        {
            _context.Subscribes.Add(sub);
            _context.SaveChanges();
        }

        public Subscribe GetSubByInstructorAndUserId(string id, string accountId)
        {
            return _context.Subscribes
                .Where(e => e.InstructorId == id && e.UserId == accountId)
                .FirstOrDefault();
        }

        public void UnSub(string id, string UserId)
        {
            var sub = _context.Subscribes
        .FirstOrDefault(s => s.InstructorId == id && s.UserId == UserId);

            if (sub != null)
            {
                _context.Subscribes.Remove(sub);
                _context.SaveChanges();
            }
        }

        List<Subscribe> ISubscriseRepository.GetSubById(string id)
        {
            return _context.Subscribes
          .Where(e => e.UserId == id)
          .ToList();
        }
    }
}