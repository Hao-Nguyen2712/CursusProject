using Cursus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Application.Credits
{
	public interface ICreditsRepository
	{
		public Domain.Models.Account AddAccMoney( string userID, int accMoney);
		public double GetAccMoney(string userID);
		public List<Domain.Models.Trading> GetAllTrading(string userID);
		public List<Domain.Models.Trading> GetAllTradingForAdmin(); // New method for admin
		public Domain.Models.Trading AddTrading(Domain.Models.Trading trading, string userID);


	}
}
