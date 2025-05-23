using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManagerMoney.Models;

public class TransactionCreateViewModel: Transaction
{
    public IEnumerable<SelectListItem> Accounts { get; set; }
    public IEnumerable<SelectListItem> Categories { get; set; }
    
}