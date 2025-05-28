using AutoMapper;
using ManagerMoney.Models;
using ManagerMoney.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ManagerMoney.Controllers;

public class AccountController : Controller
{
    private readonly IAccountTypeRepository _accountTypeRepository;
    private readonly IUserServices _userServices;
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IReportService _reportService;

    public AccountController(IAccountTypeRepository accountTypeRepository, 
                            IUserServices userServices,
                            IAccountRepository accountRepository,
                            IMapper mapper,
                            ITransactionRepository transactionRepository,
                            IReportService reportService)
    {
        _accountTypeRepository = accountTypeRepository;
        _userServices = userServices;
        _accountRepository = accountRepository;
        _mapper = mapper;
        _transactionRepository = transactionRepository;
        _reportService = reportService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userServices.GetUserId();
        var accountWithAccountType = await _accountRepository.FindByUserId(userId);

        var model = accountWithAccountType
            .GroupBy(x => x.AccountType)
            .Select(group => new IndexAccountViewModel
            {
                AccountType = group.Key,
                Accounts = group.AsEnumerable()
            }).ToList();
        
        return View(model);
    }

    public async Task<IActionResult> Detail(int id, int month, int year)
    {
        var userId = _userServices.GetUserId();
        var account = await _accountRepository.GetById(id, userId);
        
        if (account is null)
            return RedirectToAction("Page404", "Home");

        var model = await _reportService.GetTransactionReportPerAccount(userId, account.Id, month, year,ViewBag);
        ViewBag.Account = account.Name;
        
        return View(model);
    }
    
   [HttpGet]
   public async Task<IActionResult> Create( )
   {
       var userId = _userServices.GetUserId();
       var model = new AccountCreateViewModel();
       model.AccountTypes = await GetAccountTypes(userId);
       return View(model);
   }
   
   [HttpPost]
   public async Task<IActionResult> Create(AccountCreateViewModel model)
   {
       var userId = _userServices.GetUserId();
       var accountType = await _accountTypeRepository.GetById(model.AccountTypeId, userId);

       if (accountType is null)
       {
           return RedirectToAction("Page404", "Home");
       }

       if (!ModelState.IsValid)
       {
           model.AccountTypes = await GetAccountTypes(userId);
           return View(model);
       }

       await _accountRepository.Create(model);
       return RedirectToAction("Index");
   }

   public async Task<IActionResult> Edit(int id)
   {
       var userId = _userServices.GetUserId();
       var account = await _accountRepository.GetById(id, userId);
       
       if(account is null)
            return RedirectToAction("Page404", "Home");

       var model = _mapper.Map<AccountCreateViewModel>(account);
       
       model.AccountTypes = await GetAccountTypes(userId);
       return View(model);
   }

   [HttpPost]
   public async Task<IActionResult> Edit(AccountCreateViewModel accountEdit)
   {
       var userId = _userServices.GetUserId();
       var account = await _accountRepository.GetById(accountEdit.Id, userId);
       
       if(account is null)
           return RedirectToAction("Page404", "Home");
       
       var accountType = await _accountTypeRepository.GetById(accountEdit.AccountTypeId, userId);
       
       if (accountType is null)
           return RedirectToAction("Page404", "Home");
       
       await _accountRepository.Update(accountEdit);
       return RedirectToAction("Index");
   }

   [HttpGet]
   public async Task<IActionResult> Delete(int id)
   {
       var userId = _userServices.GetUserId();
       var account = await _accountRepository.GetById(id, userId);
       
       if(account is null)
           return RedirectToAction("Page404", "Home");
       
       return View(account);
   }

   [HttpPost]
   public async Task<IActionResult> DeleteAccount(int id)
   {
       var userId = _userServices.GetUserId();
       var account = await _accountRepository.GetById(id, userId);
       
       if(account is null)
           return RedirectToAction("Page404", "Home");
       
       await _accountRepository.Delete(id);
       return RedirectToAction("Index");
   }
   
   public async Task<IEnumerable<SelectListItem>> GetAccountTypes(int userId)
   {
       var accountTypes = await _accountTypeRepository.GetAll(userId);
       return accountTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
   }
   
}