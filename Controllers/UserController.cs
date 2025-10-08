using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Services.Interfaces;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult Index()
    {
        var users = _userService.GetAllUsers();
        return View(users);
    }
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var isDeleted = _userService.DeleteUser(id);
        return isDeleted ? Ok() : BadRequest();
    }

    [HttpPost]
    public IActionResult ToggleAdmin(int id)
    {
        var success = _userService.ToggleAdmin(id);
        return success ? Ok() : BadRequest();
    }

}
