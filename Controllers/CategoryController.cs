using ManagerMoney.Models;
using ManagerMoney.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManagerMoney.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IUserServices _userServices;

    public CategoryController(ICategoriesRepository categoriesRepository,
                              IUserServices userServices)
    {
        _categoriesRepository = categoriesRepository;
        _userServices = userServices;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userServices.GetUserId();
        var categories = await _categoriesRepository.GetAll(userId);
        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        if(!ModelState.IsValid)
            return View(category);
        
        var userId = _userServices.GetUserId();
        category.UserId = userId;
        await _categoriesRepository.Create(category);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var userId = _userServices.GetUserId();
        var category = await _categoriesRepository.GetById(id, userId);
        if (category is null)
            return RedirectToAction("Page404", "Home");
        
        return View(category);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(Category categoryEdit)
    {
        if (!ModelState.IsValid)
            return View(categoryEdit);
        
        var userId = _userServices.GetUserId();
        var category = await _categoriesRepository.GetById(categoryEdit.Id, userId);
        
        if (category is null)
            return RedirectToAction("Page404", "Home");
        
        categoryEdit.UserId = userId;
        await _categoriesRepository.Update(categoryEdit);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var userId = _userServices.GetUserId();
        var category = await _categoriesRepository.GetById(id, userId);
        if (category is null)
            return RedirectToAction("Page404", "Home");
        
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var userId = _userServices.GetUserId();
        var category = await _categoriesRepository.GetById(id, userId);
        
        if (category is null)
            return RedirectToAction("Page404", "Home");
        
        await _categoriesRepository.Delete(id);
        return RedirectToAction("Index");
    }
}