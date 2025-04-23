using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.Subscription;
using Cursus.Domain.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Cursus.Domain.ViewModels;

namespace Cursus.Infrastructure.Subscription
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly CursusDBContext dBContext;

        public SubscriptionRepository(CursusDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public List<Subscribe> GetSubscribes(string instructorId)
        {
            return dBContext.Subscribes.Where(s => s.InstructorId == instructorId).ToList();
        }

        public async Task<List<Cursus.Domain.Models.Account>> ListSubscription(string instructorId)
        {
             return await dBContext.Accounts
                .Where(a => a.Role == 2 && a.Id == instructorId)
                .ToListAsync();
        }
        public void SubscribeToInstructor(string instructorId, string userId)
        {
            var subscription = new Subscribe
            {
                InstructorId = instructorId,
                UserId = userId,
            };

            dBContext.Subscribes.Add(subscription);
            dBContext.SaveChanges();
        }
		public List<Domain.Models.InstructorSubscription> GetTopSubscribedInstructors(int top)
        {
			var result = dBContext.Subscribes
			.GroupBy(s => s.InstructorId)
			.Select(g => new InstructorSubscription
			{
				InstructorId = g.Key,
				SubscriptionCount = g.Count()
			})
			.OrderByDescending(x => x.SubscriptionCount)
			.Take(top)
			.ToList();
			return result;
		}
        public HomePageView GetData(int accountId, string id)
        {
             var subs = dBContext.Subscribes
                .Where(s => s.UserId == id)
                .ToList();

            var instructorIds = subs.Select(s => s.InstructorId).Distinct().ToList();
            var instructors = dBContext.Accounts
                .Where(a => instructorIds.Contains(a.Id))
                .ToList();

            var homePageView = new HomePageView
            {
                SubscribeInstructor = instructors
            };

            return homePageView;
        }

	}
}