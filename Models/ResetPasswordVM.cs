using System.ComponentModel.DataAnnotations;

namespace ManagerMoney.Models;

public class ResetPasswordVM
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
    public string Email { get; set; }
    [Required(ErrorMessage = "El campo {0} es requerido")]   
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public string Token { get; set; }
}