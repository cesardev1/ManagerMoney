using System.ComponentModel.DataAnnotations;

namespace ManagerMoney.Models;

public class LoginVM
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [EmailAddress(ErrorMessage = "El correo electronico es invalido")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "El campo {0} es requerido")]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}