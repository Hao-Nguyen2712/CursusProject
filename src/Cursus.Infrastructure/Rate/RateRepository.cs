using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application;
using Cursus.Domain.Models;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                var rate = _context.Rates.Find(RateId);
                if (rate != null)
                {
                    _context.Rates.Remove(rate);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting rate: {ex.Message}");
            }
        }

        public List<Rate> GetAllRate()
        {
            return _context.Rates
                .Include(r => r.Account)
                .Include(r => r.Course)
                .ToList();
        }

        public Rate GetRateById(int RateId)
        {
            return _context.Rates
                .Include(r => r.Account)
                .Include(r => r.Course)
                .FirstOrDefault(r => r.RateId == RateId);
        }

        public void UpdateRate(Rate rate)
        {
            try
            {
                _context.Rates.Update(rate);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating rate: {ex.Message}");
            }
        }
    }
}