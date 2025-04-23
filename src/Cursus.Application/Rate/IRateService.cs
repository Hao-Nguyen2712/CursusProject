using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application
{
    public interface IRateService
    {
        List<Rate> GetAllRate();
        Rate GetRateById(int RateId);
        void CreateRate(Rate rate);
        void UpdateRate(Rate rate);
        void DeleteRate(int RateId);
    }
}