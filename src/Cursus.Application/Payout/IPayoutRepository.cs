using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application.Payout
{
	public interface IPayoutRepository
	{
		public Domain.Models.Account UpdateAccMoney(string userID, int accMoney);
		public List<Domain.Models.Trading> GetAllPayout(string userID);
		public Domain.Models.Trading AddPayout(Domain.Models.Trading trading, string userID);
	}
}
