using System.Collections.Generic;

namespace ManagerMoney.Models;

public class MonthlyReportViewModel
{
    public IEnumerable<MonthlyResultDto> TransactionsPerMonth { get; set; }
    public decimal Incomes => TransactionsPerMonth.Sum(x => x.Income);
    public decimal Expenses => TransactionsPerMonth.Sum(x => x.Expense);
    public decimal Total => Incomes - Expenses;
    public int Year;
}