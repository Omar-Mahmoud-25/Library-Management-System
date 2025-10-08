using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        var users = _userService.GetAllUsers();
        return View(users);
    }
    [HttpGet]
    [Authorize]
    public IActionResult Delete(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
            return NotFound();

        var viewModel = new UserViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            JoiningDate = user.JoiningDate,
            IsAdmin = user.IsAdmin,
            IsActive = user.IsActive,
            BorrowedBooksCount = user.Borrowings?.Count ?? 0
        };

        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var isDeleted = _userService.DeleteUser(id);

        if (!isDeleted)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public IActionResult ToggleAdmin(int id)
    {
        var success = _userService.ToggleAdmin(id);
        return success ? Ok() : BadRequest();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Details(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }

        var viewModel = new UserViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            JoiningDate = user.JoiningDate,
            IsAdmin = user.IsAdmin,
            IsActive = user.IsActive,
            BorrowedBooksCount = user.Borrowings?.Count ?? 0
        };

        return View(viewModel);
    }

}
