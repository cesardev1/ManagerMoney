using System.ComponentModel.DataAnnotations;

namespace ManagerMoney.Models;

public class Transaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [Display(Name = "Fecha de transaccion")]
    [DataType(DataType.Date)]
    public DateTime TransactionDate { get; set; } = DateTime.Today;
    [Display(Name = "Monto")]
    public decimal Amount { get; set; }
    [Range(1,maximum:int.MaxValue,ErrorMessage = "Debe Seleccionar una categoria")]
    [Display(Name = "Categoria")]
    public int CategoryId { get; set; }
    [StringLength(maximumLength: 1000, ErrorMessage = "La nota no puede pasar de {1} caracteres.")]
    [Display(Name="Nota")]
    public string Note { get; set; }
    [Range(1,maximum:int.MaxValue,ErrorMessage = "Debe Seleccionar una cuenta")]
    [Display(Name = "Cuenta")]
    public int AccountId { get; set; }
    [Display(Name = "Tipo de operacion")]
    public OperationType OperationTypeId { get; set; } = OperationType.Ingreso;
    public string Account { get; set; }
    public string Category { get; set; }
}