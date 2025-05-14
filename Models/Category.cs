using System.ComponentModel.DataAnnotations;

namespace ManagerMoney.Models;

public class Category
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [StringLength(maximumLength: 50, ErrorMessage = "No puede ser mayor a {1} caracteres")]
    public string Name { get; set; }
    public OperationType OperationTypeId { get; set; }
    public int UserId { get; set; }
}