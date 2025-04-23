using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application
{
    public class RateService : IRateService
    {
        private readonly IRateRepository _rateRepository;

        public RateService (IRateRepository rateRespository){
            _rateRepository = rateRespository;
        }
        public void CreateRate(Rate rate)
        {
            _rateRepository.CreateRate(rate);
        }

        public void DeleteRate(int RateId)
        {
            throw new NotImplementedException();
        }

        public List<Rate> GetAllRate()
        {
            return _rateRepository.GetAllRate();
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