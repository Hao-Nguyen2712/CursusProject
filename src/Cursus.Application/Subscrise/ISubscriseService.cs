using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application.Subscrise
{
    public interface ISubscriseService
    {
        List<Subscribe> GetSubById(string id);
        void AddSub(Subscribe sub);
        void UnSub(string id,string UserId);
        public Subscribe GetSubByInstructorAndUserId(string id,string accountId);

    }
}