using System.ComponentModel.DataAnnotations;

namespace ManagerMoney.Models;

public class ForgotPasswordVM
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [EmailAddress(ErrorMessage = "El correo electrónico es inválido")]
    public string Email { get; set; }
}