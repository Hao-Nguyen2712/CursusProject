using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;
using Cursus.Application;
using Cursus.Application.Account;
using Cursus.Application.Subscrise;
using Cursus.Domain.ViewModels;

namespace Cursus.Application.Subscription
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ISubscriseRepository _subscriseRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IAccountRepository accountRepository, ISubscriseRepository subscriseRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _accountRepository = accountRepository;
            _subscriseRepository = subscriseRepository;
        }

        public List<Subscribe> GetSubscribes(string InstructorId)
        {
            return _subscriptionRepository.GetSubscribes(InstructorId);
        }

        public async Task<List<Cursus.Domain.Models.Account>> ListSubscription(string instructorId){
            return await _subscriptionRepository.ListSubscription(instructorId);
        }

          public void SubscribeToInstructor(string instructorId, string userId)
        {
            _subscriptionRepository.SubscribeToInstructor(instructorId, userId);
        }

        public List<Domain.Models.InstructorSubscription> GetTopSubscribedInstructors(int top)
        {
            var result = _subscriptionRepository.GetTopSubscribedInstructors(top);
            return result;
        }

        public HomePageView GetData(int accountId, string id)
        {
            var sub = _subscriseRepository.GetSubById(id).ToList();
            var subInStructorIds = sub.Select(s => s.InstructorId).ToList();
            var subIntructor = _accountRepository.GetAllAccount().Where(a => subInStructorIds.Contains(a.Id)).ToList();
            var homePageView = new HomePageView()
            {
                SubscribeInstructor = subIntructor
            };

            return homePageView;
        }

	}
}