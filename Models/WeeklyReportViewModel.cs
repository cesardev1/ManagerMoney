using System.ComponentModel.DataAnnotations;

namespace ManagerMoney.Models;

public class WeeklyReportViewModel
{
    [Display(Name = "Ingresos")]
    public decimal Incomes => TransactionPerWeek.Sum(x => x.Incomes);
    [Display(Name = "Gastos")]
    public decimal Expenses => TransactionPerWeek.Sum(x => x.Expenses);
    public decimal Total => Incomes - Expenses;
    public DateTime BaseDate { get; set; }
    public IEnumerable<WeeklyResultDto> TransactionPerWeek { get; set; }
}