using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.ViewModels;

namespace Cursus.Application.Subscription
{
    public interface ISubscriptionRepository
    {
        public List<Domain.Models.Subscribe> GetSubscribes(string InstructorId);
        Task<List<Cursus.Domain.Models.Account>> ListSubscription(string instructorId);
        void SubscribeToInstructor(string instructorId, string userId);
        public List<Domain.Models.InstructorSubscription> GetTopSubscribedInstructors(int top);
        HomePageView GetData(int accountId,string id);

	}
}