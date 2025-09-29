using Cursus.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Application.Account
{
    public interface IAccountRepository
    {
        List<Domain.Models.Account> GetAllAccount();
        void EditAccount(Cursus.Domain.Models.Account account);
        public Domain.Models.Account GetAccountIdById(string id);

        public int GetAccountIDByUserID(string userID);
        public Task<int> GetAccountIDByUserIDAsync(string userID);

        public string getAccountName(string userID);


        public Cursus.Domain.Models.Account GetAccountByUserID(string userID);
        public Cursus.Domain.Models.Account GetAccountByAccountID(int accountID);
        public string getEmail(int accountID);
         public Domain.Models.Account UpdateAccount(Domain.Models.Account account);
         public Domain.Models.Account UpdateAccountLevel(string userID);
    }
}
