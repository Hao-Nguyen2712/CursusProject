namespace Cursus.Application.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public void EditAccount(Domain.Models.Account account)
        {
            _accountRepository.EditAccount(account);
        }

        public Domain.Models.Account GetAccountByAccountID(int accountID)
        {
            return _accountRepository.GetAccountByAccountID(accountID);
        }

        public Domain.Models.Account GetAccountByUserID(string userID)
        {
            return _accountRepository.GetAccountByUserID(userID);
        }

        public Domain.Models.Account GetAccountIdById(string id)
        {
            return _accountRepository.GetAccountIdById(id);
        }

        public int GetAccountIDByUserID(string userID)
        {
            int id = _accountRepository.GetAccountIDByUserID(userID);
            return id;
        }


        public string getAccountName(string userID)
        {
            string name = _accountRepository.getAccountName(userID);
            return name;
        }

        public List<Domain.Models.Account> GetAllAccount()
        {
            return _accountRepository.GetAllAccount();

        }

        public object GetData(int accountId, string userID)
        {
            throw new NotImplementedException();
        }

        public string getEmail(int accountID)
        {
            return _accountRepository.getEmail(accountID);
        }

        public Domain.Models.Account UpdateAccount(Domain.Models.Account account)
        {
            return _accountRepository.UpdateAccount(account);
        }
        public Domain.Models.Account UpdateAccountLevel(string userID)
        {
            return _accountRepository.UpdateAccountLevel(userID);
        }
    }
}

