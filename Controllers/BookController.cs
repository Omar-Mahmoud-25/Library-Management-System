using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers;

public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _CategoriesService;
    public BookController(
            IBookService bookService,
            ICategoryService categoryService)
    {
        _bookService = bookService;
        _CategoriesService = categoryService;
    }

    public async Task<IActionResult> Index(int? categoryId, string? search)
    {
        var books = await _bookService.GetAll(categoryId, search);
        var categories = await _CategoriesService.GetSelectList();
        ViewBag.Categories = categories;
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.SearchTerm = search;
        ViewBag.IsAdmin = User.IsInRole("Admin");
        return View(books);
    }
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new CreateBookViewModel
        {
            Categories = await _CategoriesService.GetSelectList()
        };

        return View(viewModel);
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateBookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = await _CategoriesService.GetSelectList();
            return View(model);
        }

        await _bookService.Create(model);

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var book = _bookService.GetById(id);

        if (book == null)
            return NotFound();
        EditBookViewModel viewModel = new()
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            PublishedDate = book.PublishedDate,
            CopiesAvailable = book.CopiesAvailable,
            CurrentCoverImageUrl = book.CoverImageUrl,
            Categories = await _CategoriesService.GetSelectList(),
            SelectedCategoryIds = book.BookCategories.Select(bc => bc.CategoryId).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(EditBookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = await _CategoriesService.GetSelectList();
            return View(model);
        }

        var updatedBook = await _bookService.UpdateAsync(model);

        if (updatedBook == null)
            return BadRequest();

        return RedirectToAction(nameof(Index));
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var isDeleted = await _bookService.DeleteAsync(id);

        return isDeleted ? Ok() : BadRequest();
    }
    public IActionResult Details(int id)
    {
        var book = _bookService.GetById(id); 

        if (book is null)
            return NotFound();

        return View(book); 
    }


}
