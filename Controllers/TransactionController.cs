using AutoMapper;
using ManagerMoney.Models;
using ManagerMoney.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManagerMoney.Controllers;

public class TransactionController : Controller
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserServices _userServices;
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IMapper _mapper;
    private readonly IReportService _reportService;

    public TransactionController(ITransactionRepository transactionRepository,
                                 IUserServices userServices,
                                 IAccountRepository accountRepository,
                                 ICategoriesRepository categoriesRepository,
                                 IMapper mapper,
                                 IReportService reportService)
    {
        _transactionRepository = transactionRepository;
        _userServices = userServices;
        _accountRepository = accountRepository;
        _categoriesRepository = categoriesRepository;
        _mapper = mapper;
        _reportService = reportService;
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id,string urlReturn = null)
    {
        var userId = _userServices.GetUserId();
        
        var transaction = await _transactionRepository.GetById(id, userId);
        
        if (transaction is null)
            return RedirectToAction("Page404", "Home");
        
        var model = _mapper.Map<TransactionUpdateViewModel>(transaction);

        model.LastMount = model.Mount;
        
        if (model.OperationTypeId == OperationType.Gasto)
            model.LastMount = model.Mount * -1;
        
        model.LastAccountId = transaction.AccountId;
        model.Categories = await GetCategories(userId, transaction.OperationTypeId);
        model.Accounts = await GetAccounts(userId);
        model.UrlReturn = urlReturn;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id,string urlReturn)
    {
        var userId = _userServices.GetUserId();
        var transaction = await _transactionRepository.GetById(id, userId);
        
        if (transaction is null)
            return RedirectToAction("Page404", "Home");
        
        await _transactionRepository.Delete(id);
        if (string.IsNullOrEmpty(urlReturn))
        {
            return RedirectToAction("Index");
        }
        else
        {
            return LocalRedirect(urlReturn);
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TransactionUpdateViewModel model)
    {
        var userId = _userServices.GetUserId();
        if(!ModelState.IsValid)
        {
            model.Accounts = await GetAccounts(userId);
            model.Categories = await GetCategories(userId, model.OperationTypeId);
            return View(model);
        }
        
        var account = await _accountRepository.GetById(model.AccountId, userId);
        
        if(account is null)
            return RedirectToAction("Page404", "Home");
            
        var category = await _categoriesRepository.GetById(model.CategoryId, userId);
        
        if (category is null)
            return RedirectToAction("Page404", "Home");
        
        var transaction = _mapper.Map<Transaction>(model);
        if (model.OperationTypeId == OperationType.Gasto)
             transaction.Mount *= -1;
        
        await _transactionRepository.Update(transaction,model.LastMount,model.LastAccountId);
        if (string.IsNullOrEmpty(model.UrlReturn))
        {
            return RedirectToAction("Index");
        }
        else
        {
            return LocalRedirect(model.UrlReturn);
        }
        
    }
    public async Task<IActionResult> Create()
    {
        var userId = _userServices.GetUserId();
        var model = new TransactionCreateViewModel();
        
        model.Accounts = await GetAccounts(userId);
        model.Categories = await GetCategories(userId, model.OperationTypeId);
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(TransactionCreateViewModel model)
    {
        var userId = _userServices.GetUserId();
        if(!ModelState.IsValid)
        {
            model.Accounts = await GetAccounts(userId);
            model.Categories = await GetCategories(userId, model.OperationTypeId);
            return View(model);
        }
        
        var account = await _accountRepository.GetById(model.AccountId, userId);
        
        if (account is null)
        {
            return RedirectToAction("Page404", "Home");
        }
        
        var category = await _categoriesRepository.GetById(model.CategoryId, userId);
        
        if (category is null)
            return RedirectToAction("Page404", "Home");
        
        model.UserId = userId;
        if (model.OperationTypeId == OperationType.Gasto)
            model.Mount *= -1;
        
        await _transactionRepository.Create(model);
        return RedirectToAction("Index");
    }

    private async Task<IEnumerable<SelectListItem>> GetAccounts(int userId)
    {
        var accounts = await _accountRepository.FindByUserId(userId);
        return accounts.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
    }
    
    private async Task<IEnumerable<SelectListItem>> GetCategories(int userId, OperationType operationType)
    {
        var categories = await _categoriesRepository.GetAll(userId, operationType);
        return categories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
    }

    [HttpPost]
    public async Task<IActionResult> GetCategories([FromBody] OperationType operationType)
    {
        var userId = _userServices.GetUserId();
        var categories = await GetCategories(userId, operationType);
        return Ok(categories); 
    }

    public async Task<IActionResult> Index(int month, int year)
    {
        var userId = _userServices.GetUserId();
        
        var model = await _reportService.GetTransactionDetailReport(userId, month, year, ViewBag);
        
        return View(model);
    }
}