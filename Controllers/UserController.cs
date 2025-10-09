using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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
    
    [HttpPost]
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

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userProfile = await _userService.GetUserProfileAsync(userId);
        
        if (userProfile == null)
        {
            return NotFound();
        }
        
        return View(userProfile);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(UserProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        // Ensure user can only edit their own profile (or admin can edit any)
        if (model.Id != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var success = await _userService.UpdateUserProfileAsync(model);
        
        if (success)
        {
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction(nameof(Profile));
        }
        else
        {
            ModelState.AddModelError("", "An error occurred while updating your profile.");
            return View(model);
        }
    }

    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var success = await _userService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);
        
        if (success)
        {
            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToAction(nameof(Profile));
        }
        else
        {
            ModelState.AddModelError("", "Current password is incorrect.");
            return View(model);
        }
    }
}
