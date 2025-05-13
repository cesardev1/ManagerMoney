using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManagerMoney.Models;

public class AccountCreateViewModel : Account
{
    public IEnumerable<SelectListItem> AccountTypes { get; set; }
}