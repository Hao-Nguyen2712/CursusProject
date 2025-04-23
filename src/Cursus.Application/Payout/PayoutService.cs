using Cursus.Application.Credits;
using Cursus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Application.Payout
{
	public class PayoutService : IPayoutService
	{
		private readonly IPayoutRepository _payoutRepository;

		public PayoutService(IPayoutRepository payoutRepository)
		{
			_payoutRepository = payoutRepository;
		}

		public Trading AddPayout(Trading trading, string userID)
		{
			if (trading == null)
			{
				return null;
			}
			_payoutRepository.AddPayout(trading, userID);
			return trading;
		}

		public List<Trading> GetAllPayout(string userID)
		{
			var listPayout = _payoutRepository.GetAllPayout(userID);
			return listPayout;
		}

		public Domain.Models.Account UpdateAccMoney(string userID, int accMoney)
		{
			return _payoutRepository.UpdateAccMoney(userID, accMoney);
		}
	}
}
