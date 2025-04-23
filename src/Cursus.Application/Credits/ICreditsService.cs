using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cursus.Application.Models;
using Cursus.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json.Linq;

namespace Cursus.Application.Credits
{
    public interface ICreditsService
    {
        string CreateRequestUrl(double money, string bankCode);
        VnPaymentResponseViewModel PaymentExcute(IQueryCollection collection);
        Task<double> ConvertUSDToVND(double usdAmount);
		public Domain.Models.Account AddAccMoney(string userID, int accMoney);
        public double GetAccMoney(string userID);
		public List<Domain.Models.Trading> GetAllTrading(string userID);
		public Domain.Models.Trading AddTrading(Domain.Models.Trading trading, string userID);



	}
}
