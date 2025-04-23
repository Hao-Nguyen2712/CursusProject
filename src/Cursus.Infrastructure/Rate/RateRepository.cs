using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application;
using Cursus.Domain.Models;

namespace Cursus.Infrastructure
{
    public class RateRepository : IRateRepository
    {
        private readonly CursusDBContext _context;

        public RateRepository (CursusDBContext context){
            _context = context;
        }
        public void CreateRate(Rate rate)
        {
            _context.Rates.Add(rate);
            _context.SaveChanges();
        }

        public void DeleteRate(int RateId)
        {
            throw new NotImplementedException();
        }

        public List<Rate> GetAllRate()
        {
            return _context.Rates.ToList();
        }

        public Rate GetRateById(int RateId)
        {
            throw new NotImplementedException();
        }

        public void UpdateRate(Rate rate)
        {
            throw new NotImplementedException();
        }
    }
}