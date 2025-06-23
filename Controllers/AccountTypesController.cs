using ManagerMoney.Models;
using ManagerMoney.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


namespace ManagerMoney.Controllers
{
    public class AccountTypesController : Controller
    {
        private readonly IAccountTypeRepository _accountTypeRepository;
        private readonly IUserServices _userServices;

        public AccountTypesController(IAccountTypeRepository accountTypeRepository, IUserServices userServices)
        {
            _accountTypeRepository = accountTypeRepository;
            _userServices = userServices;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userServices.GetUserId();
            var accountTypes = await _accountTypeRepository.GetAll(userId);
            return View(accountTypes);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userServices.GetUserId();
            var accountType = await _accountTypeRepository.GetById(id, userId);

            if (accountType == null)
            {

                return RedirectToAction("Page404", "Home");
            }

            return View(accountType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountType accountType)
        {
            if (!ModelState.IsValid)
            {
                return View(accountType);
            }

            var userId = _userServices.GetUserId();
            // Verificamos que el tipo de cuenta exista en la base de datos 
            var accountTypeToDatabase = await _accountTypeRepository.GetById(accountType.Id,userId);


            if (accountTypeToDatabase is null)
                return RedirectToAction("Page404", "Home");

            await _accountTypeRepository.Update(accountType);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountType accountType)
        {
            if (!ModelState.IsValid)
            {
                return View(accountType);
            }
            accountType.UserId = _userServices.GetUserId();

            var validationExist = await _accountTypeRepository.Exist(accountType.Name, accountType.UserId);

            if(validationExist)
            {
                ModelState.AddModelError(nameof(accountType.Name), $"El nombre {accountType.Name} ya existe");
                return View(accountType);
            }

            await _accountTypeRepository.Create(accountType);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ValidateExistAccountType(string name)
        {
            var usuarioId = _userServices.GetUserId();
            var exist = await _accountTypeRepository.Exist(name, usuarioId);

            if (exist)
            {
                return Json($"El nombre {name} ya existe");
            }
            else
            {
                return Json(true);
            }
        }
       
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userServices.GetUserId();
            var validateExistAccountType= await _accountTypeRepository.GetById(id, userId);

            if (validateExistAccountType is null)
            {
                return RedirectToAction("Page404", "Home");                
            }

            return View(validateExistAccountType);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccountType(int id)
        {
            var userId = _userServices.GetUserId();
            var validateExistAccountType= await _accountTypeRepository.GetById(id, userId);

            if (validateExistAccountType is null)
            {
                return RedirectToAction("Page404", "Home");                
            }
            
            await _accountTypeRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Order([FromBody]int [] ids)
        {
            var userId = _userServices.GetUserId();
            var accountTypes = await _accountTypeRepository.GetAll(userId);
            var idsAccountTypes = accountTypes.Select(x => x.Id);

            var idsAccountTypesValidateUser = ids.Except(idsAccountTypes).ToList();

            if (idsAccountTypesValidateUser.Count > 0)
                return Forbid();

            var accountTypesOrder = ids.Select((value,index)=>
                                        new AccountType() {Id = value,OrderIndex = index + 1}).AsEnumerable();
            
            await _accountTypeRepository.Order(accountTypesOrder);
            
            return Ok();
        }
        
    }
    
}