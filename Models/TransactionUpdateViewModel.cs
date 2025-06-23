namespace ManagerMoney.Models;

public class TransactionUpdateViewModel: TransactionCreateViewModel
{
    public int PreviousAccountId { get; set; }
    public decimal PreviousAmount { get; set; }
    public string UrlReturn { get; set; }
}