using System.Runtime.InteropServices.JavaScript;
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

        model.LastAmount = model.Amount;
        
        if (model.OperationTypeId == OperationType.Gasto)
            model.LastAmount = model.Amount * -1;
        
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
             transaction.Amount *= -1;
        
        await _transactionRepository.Update(transaction,model.LastAmount,model.LastAccountId);
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
            model.Amount *= -1;
        
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
    
    // Reports section: the "Index" refers to the daily report

    public async Task<IActionResult> Index(int month, int year)
    {
        var userId = _userServices.GetUserId();
        
        var model = await _reportService.GetTransactionDetailReport(userId, month, year, ViewBag);
        
        return View(model);
    }

    public async Task<IActionResult> WeeklyReport(int month, int year)
    {
        var userId = _userServices.GetUserId();
        IEnumerable<WeeklyResultDto> transactionsWeek = await  _reportService.GetTransactionReportPerWeek(userId, month,year,ViewBag);
        
        var grouped = transactionsWeek.GroupBy(x=> x.Week).Select(x =>
                    new WeeklyResultDto()
                    {
                        Week = x.Key,
                        Incomes = x.Where(w=> w.OperationTypeId == OperationType.Ingreso).Select(o=> o.Amount).FirstOrDefault(),
                        Expenses = x.Where(w=> w.OperationTypeId == OperationType.Gasto).Select(o=> o.Amount).FirstOrDefault()
                    }).ToList();

        if (year == 0 || month == 0)
        {
            var today = DateTime.Today;
            
            year = today.Year;
            month = today.Month;
        }

        var refDate = new DateTime(year,month,1);
        var daysOfMonth = Enumerable.Range(1, refDate.AddMonths(1).AddDays(-1).Day);

        var segmentedDays = daysOfMonth.Chunk(7).ToList();

        for (int i = 0; i < segmentedDays.Count(); i++)
        {
            var week = i + 1;
            var startDate = new DateTime(year,month,segmentedDays[i].First());
            var endDate = new DateTime(year,month,segmentedDays[i].Last());
            var groupWeek = grouped.FirstOrDefault(x => x.Week == week);

            if (groupWeek is null)
            {
                grouped.Add(new WeeklyResultDto()
                {
                    Week =  week,
                    StartDate = startDate,
                    EndDate = endDate
                });
            }
            else
            {
                groupWeek.StartDate = startDate;
                groupWeek.EndDate = endDate;
            }
        }
        
        grouped = grouped.OrderByDescending(x=>x.Week).ToList();

        var model = new WeeklyReportViewModel();
        model.TransactionPerWeek = grouped;
        model.BaseDate = refDate;
        return View(model);
    }
    public IActionResult MonthlyReport()
    {
        return View();
    }
    public IActionResult ExcelReport()
    {
        return View();
    }
    public IActionResult Calendar()
    {
        return View();
    }
}