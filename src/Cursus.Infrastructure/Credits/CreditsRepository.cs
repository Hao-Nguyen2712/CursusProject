using Cursus.Application.Credits;
using Cursus.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Infrastructure.Credits
{
	public class CreditsRepository : ICreditsRepository
	{
		private readonly CursusDBContext _dbContext;

		public CreditsRepository(CursusDBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public Domain.Models.Account AddAccMoney( string userID,int accMoney)
		{
			Domain.Models.Account account = _dbContext.Accounts.FirstOrDefault(x => x.Id == userID);
			if (account == null)
			{
				return null;
			}
			if(account.Money == null)
			{
				return null;
			}
			account.Money += accMoney;
			_dbContext.Update(account);
			_dbContext.SaveChanges();
			return account;
		}

		public double GetAccMoney(string userID)
		{
			Domain.Models.Account account = _dbContext.Accounts.FirstOrDefault(x => x.Id == userID);
			if (account == null)
			{
				return 0; 
			}
			decimal nonNullableDecimal = account.Money.GetValueOrDefault();
			double accMoney = decimal.ToDouble(nonNullableDecimal);
			return accMoney;
		}

		public List<Domain.Models.Trading> GetAllTrading(string userID)
		{
			Domain.Models.Account account = _dbContext.Accounts.FirstOrDefault(x => x.Id == userID);
			if (account == null)
			{
				return null;
			}
			var tradings = _dbContext.Tradings.Where(t => t.AccountId == account.AccountId  && t.TdMethodPayment != "Payout").ToList();
            if (tradings.Count == 0)
            {
                return null;
            }
            return tradings;
		}

		public List<Domain.Models.Trading> GetAllTradingForAdmin()
		{
			var tradings = _dbContext.Tradings
				.Include(t => t.Account)
				.OrderByDescending(t => t.TdDate)
				.ToList();
            return tradings;
		}

		public Domain.Models.Trading AddTrading(Domain.Models.Trading trading, string userID)
		{
			Domain.Models.Account account = _dbContext.Accounts.FirstOrDefault(x => x.Id == userID);
			if (account == null)
			{
				return null;
			}
			if (trading == null) {  
				return null; 
			}
			trading.AccountId = account.AccountId;
			_dbContext.Add(trading);
			_dbContext.SaveChanges();
			return trading;

		}

	}
}
