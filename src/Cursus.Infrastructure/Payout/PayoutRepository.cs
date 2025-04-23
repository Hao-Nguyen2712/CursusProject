using Cursus.Application.Payout;
using Cursus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Infrastructure.Payout
{
	public class PayoutRepository : IPayoutRepository
	{
		private readonly CursusDBContext _dbContext;

		public PayoutRepository(CursusDBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public Domain.Models.Account UpdateAccMoney(string userID, int accMoney)
		{
			Domain.Models.Account account = _dbContext.Accounts.FirstOrDefault(x => x.Id == userID);
			if (account == null)
			{
				return null;
			}
			if(account.Money < accMoney)
			{
				return null; 
			}
			account.Money -= accMoney;
			_dbContext.Update(account);
			_dbContext.SaveChanges();
			return account;
		}

		public Domain.Models.Trading AddPayout(Domain.Models.Trading trading, string userID)
		{
			Domain.Models.Account account = _dbContext.Accounts.FirstOrDefault(x => x.Id == userID);
			if (account == null)
			{
				return null;
			}
			if (trading == null)
			{
				return null;
			}
			trading.TdMethodPayment = "Payout";
			trading.AccountId = account.AccountId;
			_dbContext.Add(trading);
			_dbContext.SaveChanges();
			return trading;
		}

		public List<Domain.Models.Trading> GetAllPayout(string userID)
		{
			Domain.Models.Account account = _dbContext.Accounts.FirstOrDefault(x => x.Id == userID);
			if (account == null)
			{
				return null;
			}
			var tradings = _dbContext.Tradings.Where(t => t.AccountId == account.AccountId && t.TdMethodPayment == "Payout").ToList();
            if (tradings.Count == 0)
            {
                return null;
            }
            return tradings;
		}
	}
}
