using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers;

[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: Category
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        var categoryViewModels = categories.Select(c => new CategoryViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            BooksCount = c.BookCategories?.Count ?? 0
        }).ToList();

        return View(categoryViewModels);
    }

    // GET: Category/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var categoryViewModel = new CategoryViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            BooksCount = category.BookCategories?.Count ?? 0
        };

        return View(categoryViewModel);
    }

    // GET: Category/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Category/Create
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateCategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var category = new Category
        {
            Name = model.Name,
            Description = model.Description ?? string.Empty
        };

        var result = await _categoryService.AddCategoryAsync(category);
        if (result)
        {
            TempData["SuccessMessage"] = "Category created successfully!";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("Name", "A category with this name already exists.");
        return View(model);
    }

    // GET: Category/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var editViewModel = new EditCategoryViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };

        return View(editViewModel);
    }

    // POST: Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, EditCategoryViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var category = new Category
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description ?? string.Empty
        };

        var result = await _categoryService.UpdateCategoryAsync(category);
        if (result)
        {
            TempData["SuccessMessage"] = "Category updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("Name", "A category with this name already exists or the category was not found.");
        return View(model);
    }

    // DELETE: Category/Delete/5
    [HttpDelete]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);
        if (result)
        {
            TempData["SuccessMessage"] = "Category deleted successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to delete category. It may be associated with books.";
        }

        return RedirectToAction(nameof(Index));
    }

    // AJAX endpoint for checking if category name exists
    [HttpPost]
    public async Task<JsonResult> CheckCategoryName(string name, int? id = null)
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        var exists = categories.Any(c => 
            c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
            (id == null || c.Id != id));

        return Json(!exists);
    }
}
