namespace ManagerMoney.Models;

public class IndexAccountViewModel
{
    public string AccountType { get; set; }
    public IEnumerable<Account> Accounts { get; set; }
    public decimal Balance => Accounts.Sum(x => x.Balance);
}