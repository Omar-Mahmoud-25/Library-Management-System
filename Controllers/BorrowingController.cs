using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Services.Interfaces;

public class BorrowingController : Controller
{
    private readonly IBorrowingService _borrowingService;

    public BorrowingController(IBorrowingService borrowingService)
    {
        _borrowingService = borrowingService;
    }

    public IActionResult Index()
    {
        var borrowings = _borrowingService.GetAllBorrowings();

        ViewBag.BorrowedCount = _borrowingService.GetBorrowedCount();
        ViewBag.OverdueCount = _borrowingService.GetOverdueCount();
        ViewBag.ReturnedCount = _borrowingService.GetReturnedCount();

        return View(borrowings);
    }

}
