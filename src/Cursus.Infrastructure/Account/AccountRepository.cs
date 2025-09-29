using Cursus.Application.Account;
using Cursus.Domain.Models;
using Microsoft.EntityFrameworkCore;

public class AccountRepository : IAccountRepository
{
    private readonly CursusDBContext _db;

    public AccountRepository(CursusDBContext db)
    {


        _db = db;
    }

    public int getAccountIDByUserID(string userID)
    {
        int id = _db.Accounts.AsNoTracking()
            .Where(a => a.Id == userID)
            .Select(a => a.AccountId)
            .FirstOrDefault();
        return id;
    }

    public string getAccountName(string userID)
    {
        string name = _db.Accounts.AsNoTracking()
            .Where(a => a.Id == userID)
            .Select(a => a.FullName)
            .FirstOrDefault();
        return name;
    }

    /// <summary>
    /// Edits the specified account in the database.
    /// </summary>
    /// <param name="account">The account to be edited.</param>
    public void EditAccount(Account account)
    {
        // Update the account in the database
        _db.Accounts.Update(account);
        // Save the changes to the database
        _db.SaveChanges();
    }

    public Cursus.Domain.Models.Account GetAccountByAccountID(int accountID)
    {
        return _db.Accounts.AsNoTracking()
            .Where(a => a.AccountId == accountID)
            .FirstOrDefault();
    }
    public Cursus.Domain.Models.Account GetAccountByUserID(string userID)
    {
        Cursus.Domain.Models.Account account = _db.Accounts.AsNoTracking()
            .Where(a => a.Id == userID)
            .FirstOrDefault();
        return account;
    }

    public async Task<int> GetAccountIDByUserIDAsync(string userID)
    {
        int id = await _db.Accounts.AsNoTracking()
            .Where(a => a.Id == userID)
            .Select(a => a.AccountId)
            .FirstOrDefaultAsync();
        return id;
    }

    // Keep synchronous version for backward compatibility if needed
    public int GetAccountIDByUserID(string userID)
    {
        int id = _db.Accounts.AsNoTracking()
            .Where(a => a.Id == userID)
            .Select(a => a.AccountId)
            .FirstOrDefault();
        return id;
    }
    public Account GetAccountIdById(string id)
    {
        return _db.Accounts.Where(e => e.Id == id)
          .FirstOrDefault();
    }
    public List<Account> GetAllAccount()
    {
        return _db.Accounts.ToList();
    }

    public string getEmail(int accountID)
    {
        return _db.Accounts.Where(a => a.AccountId == accountID).Select(a => a.Email).FirstOrDefault();
    }

    public Cursus.Domain.Models.Account UpdateAccount(Cursus.Domain.Models.Account account)
    {
        Cursus.Domain.Models.Account account1 = _db.Accounts.Where(a => a.AccountId == account.AccountId).FirstOrDefault();
        if (account1 == null)
        {
            return null;
        }
        // update account
        account1.Avatar = account.Avatar;
        _db.Accounts.Update(account1);
        _db.SaveChanges();

        return account1;
    }

    public Cursus.Domain.Models.Account UpdateAccountLevel(string userID)
    {
        Cursus.Domain.Models.Account account = _db.Accounts.Where(a => a.Id == userID).FirstOrDefault();
        if (account == null)
        {
            return null;
        }
        // update account
        account.UpLevel = "True";
        _db.Accounts.Update(account);
        _db.SaveChanges();
        return account;
    }
}
