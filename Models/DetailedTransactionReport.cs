namespace ManagerMoney.Models;

public class DetailedTransactionReport
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IEnumerable<TransactionPerDate> GroupedTransactions { get; set; }
    public decimal TotalDeposit => GroupedTransactions.Sum(x => x.DepositBalance);
    public decimal TotalWithdraw => GroupedTransactions.Sum(x => x.WithdrawBalance);
    public decimal TotalBalance => TotalDeposit - TotalWithdraw;

    public class TransactionPerDate
    {
        public DateTime TransactionDate { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }

        public decimal DepositBalance => Transactions.Where(x => x.OperationTypeId == OperationType.Ingreso)
                                                     .Sum(x => x.Amount);

        public decimal WithdrawBalance => Transactions.Where(x => x.OperationTypeId == OperationType.Gasto)
                                                      .Sum(x=> x.Amount);
        
        
    }
}