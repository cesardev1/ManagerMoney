using ManagerMoney.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManagerMoney.Models
{
    public class AccountType
    {
 
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo nombre es obligatorio")]
        [FirstLetterUpper]
        [Remote(action: "ValidateExistAccountType", controller: "AccountTypes")]
        public string Name { get; set; }
        public int UserId { get; set; }
        public int OrderIndex { get; set; }
    }
}