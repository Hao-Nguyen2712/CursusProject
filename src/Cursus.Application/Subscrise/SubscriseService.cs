using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.Subscrise;
using Cursus.Domain.Models;
using Cursus.Application;

namespace Cursus.Application.Subscrise
{
    public class SubscriseService : ISubscriseService
    {
        private readonly ISubscriseRepository _subscriseRepository;
        public SubscriseService(ISubscriseRepository subscriseRepository){
            _subscriseRepository = subscriseRepository;
        }

        public void AddSub(Subscribe sub)
        {
             _subscriseRepository.AddSub(sub);
        }

        public Subscribe GetSubByInstructorAndUserId(string id,string accountId)
        {
            return _subscriseRepository.GetSubByInstructorAndUserId(id,accountId);
        }

        public void UnSub(string id, string UserId)
        {
            _subscriseRepository.UnSub(id,UserId);
        }

        List<Subscribe> ISubscriseService.GetSubById(string id)
        {
            return _subscriseRepository.GetSubById(id);
        }
    }
}