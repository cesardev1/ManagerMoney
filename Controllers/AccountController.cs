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

    public AccountController(IAccountTypeRepository accountTypeRepository, 
                            IUserServices userServices,
                            IAccountRepository accountRepository,
                            IMapper mapper,
                            ITransactionRepository transactionRepository)
    {
        _accountTypeRepository = accountTypeRepository;
        _userServices = userServices;
        _accountRepository = accountRepository;
        _mapper = mapper;
        _transactionRepository = transactionRepository;
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

        DateTime startDate;
        DateTime endDate;

        if (month <= 0 || month > 12 || year <= 1900)
        {
            var today = DateTime.Today;
            startDate = new DateTime(today.Year, today.Month, 1);
        }
        else
        {
            startDate = new DateTime(year, month, 1);
        }
        endDate = startDate.AddMonths(1).AddDays(-1);

        var getTransactionByAccount = new GetTransactionsByAccount()
        {
            AccountId = id,
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate
        };
        
        var transactions = await _transactionRepository.GetAllByAccountId(getTransactionByAccount);
        var model = new DetailedTransactionReport();
        ViewBag.Account = account.Name;

        var transactionsPerDate = transactions.OrderByDescending(x => x.DateTransaction)
            .GroupBy(x => x.DateTransaction)
            .Select(group => new DetailedTransactionReport.TransactionPerDate()
            {
                TransactionDate = group.Key,
                Transactions = group.AsEnumerable()
            });
        
        model.GroupedTransactions = transactionsPerDate;
        model.StartDate = startDate;
        model.EndDate = endDate;
        
        ViewBag.LastMonth = startDate.AddMonths(-1).Month;
        ViewBag.LastYear = startDate.AddMonths(-1).Year;
        ViewBag.NextMonth = startDate.AddMonths(1).Month;
        ViewBag.NextYear = startDate.AddMonths(1).Year;
        ViewBag.urlReturn = HttpContext.Request.Path + HttpContext.Request.QueryString;
        
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