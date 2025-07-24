using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Pkcs;

namespace ManagerMoney.Models;

public class SignUpVM
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [EmailAddress(ErrorMessage = "El correo electronico es invalido")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "El campo {0} es requerido")]
    public string Password { get; set; }
}