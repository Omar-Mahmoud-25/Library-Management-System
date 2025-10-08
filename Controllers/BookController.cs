using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

    public async Task<IActionResult> Index()
    {
        var books = await _bookService.GetAll();
        ViewBag.IsAdmin = User.IsInRole("Admin");
        return View(books);
    }
    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new CreateBookViewModel
        {
            Categories = _CategoriesService.GetSelectList()
        };

        return View(viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = _CategoriesService.GetSelectList();
            return View(model);
        }

        await _bookService.Create(model);

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var book = _bookService.GetById( id);

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
            CoverImageUrl = book.CoverImageUrl,
            Categories = _CategoriesService.GetSelectList(),
            SelectedCategoryId = book.BookCategories.FirstOrDefault()?.CategoryId ?? 0
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditBookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = _CategoriesService.GetSelectList();
            return View(model);
        }

        var updatedBook = await _bookService.UpdateAsync(model);

        if (updatedBook == null)
            return BadRequest();

        return RedirectToAction(nameof(Index));
    }

    [HttpDelete]
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
