using System.ComponentModel.DataAnnotations;
using ManagerMoney.Validations;

namespace ManagerMoney.Models;

public class Account
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El campo nombre es obligatorio")]
    [StringLength(maximumLength: 50)]
    [FirstLetterUpper]
    [Display(Name = "Nombre")]
    public string Name { get; set; }
    [Display(Name = "Tipo de cuenta")]
    public int AccountTypeId { get; set; }
    [Display(Name = "Saldo")]
    public decimal Balance { get; set; }
    [Display(Name = "Descripccion")]
    public string Description { get; set; }

    public string AccountType { get; set; }
}