using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LibraryManagementSystem.Controllers;

public class BorrowingController : Controller
{
    private readonly IBorrowingService _borrowingService;
    private readonly IBookService _bookService;

    public BorrowingController(IBorrowingService borrowingService, IBookService bookService)
    {
        _borrowingService = borrowingService;
        _bookService = bookService;
    }

    // Admin view - All borrowings with filtering
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index(string? status, string? search)
    {   
        var borrowings = await _borrowingService.GetAllBorrowingsAsync();


        if (!string.IsNullOrEmpty(status))
        {
            switch (status.ToLower())
            {
                case "active":
                    borrowings = borrowings.Where(b => b.ReturnedDate == null).ToList();
                    break;
                case "returned":
                    borrowings = borrowings.Where(b => b.ReturnedDate != null).ToList();
                    break;
                case "overdue":
                    borrowings = borrowings.Where(b => b.IsOverdue).ToList();
                    break;
            }
        }

        if (!string.IsNullOrEmpty(search))
        {
            borrowings = borrowings.Where(b => 
                b.Book.Title.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                b.User.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                b.User.Email.Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        // Statistics
        ViewBag.BorrowedCount = await _borrowingService.GetBorrowedCountAsync();
        ViewBag.OverdueCount = await _borrowingService.GetOverdueCountAsync();
        ViewBag.ReturnedCount = await _borrowingService.GetReturnedCountAsync();
        ViewBag.TotalCount = ViewBag.BorrowedCount + ViewBag.ReturnedCount;
        ViewBag.IsAdmin = true;

        return View(borrowings);
    }

    // User view - My borrowings
    [Authorize]
    public async Task<IActionResult> MyBorrowings(string? status)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var borrowings = await _borrowingService.GetUserBorrowingsAsync(userId);

        if (!string.IsNullOrEmpty(status))
        {
            switch (status.ToLower())
            {
                case "active":
                    borrowings = borrowings.Where(b => b.ReturnedDate == null).ToList();
                    break;
                case "returned":
                    borrowings = borrowings.Where(b => b.ReturnedDate != null).ToList();
                    break;
                case "overdue":
                    borrowings = borrowings.Where(b => b.IsOverdue).ToList();
                    break;
            }
        }   

        ViewBag.IsAdmin = User.IsInRole("Admin");
        
        return View(borrowings);
    }

    // Book-specific borrowings (for book details page)
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BookHistory(int bookId)
    {
        var borrowings = await _borrowingService.GetBookBorrowingsAsync(bookId);
        var book = _bookService.GetById(bookId);
        
        if (book == null)
        {
            return NotFound();
        }

        ViewBag.BookTitle = book.Title;
        ViewBag.BookId = bookId;
        
        return View(borrowings);
    }

    // Borrow a book
    [Authorize]
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow([FromRoute(Name = "id")] int bookId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        // Check if user can borrow
        if (!await _borrowingService.CanUserBorrowBookAsync(bookId, userId))
        {
            TempData["ErrorMessage"] = "Unable to borrow this book. It might be unavailable or you have reached your borrowing limit (5 books maximum).";
            return RedirectToAction("Index", "Book", new { id = bookId });
        }

        var result = await _borrowingService.BorrowBookAsync(bookId, userId);
        
        if (result)
        {
            TempData["SuccessMessage"] = "Book borrowed successfully! Due date is 14 days from now.";
            return RedirectToAction("MyBorrowings");
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to borrow the book. Please try again.";
            return RedirectToAction("Details", "Book", new { id = bookId });
        }
    }

    // Return a book
    [HttpPost]
    [Authorize]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(int bookId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        // var borrowing = await _borrowingService.GetBorrowingByIdAsync(bookId);
        // if (borrowing == null || borrowing.UserId != userId)
        // {
        //     return NotFound();
        // }

        var result = await _borrowingService.ReturnBookAsync(bookId, userId);
        
        if (result)
        {
            TempData["SuccessMessage"] = "Book returned successfully!";
            return RedirectToAction("MyBorrowings");
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to return the book. Please try again.";
            return RedirectToAction("MyBorrowings");
        }
    }

    // Get borrowing details
    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var borrowing = await _borrowingService.GetBorrowingByIdAsync(id);
        
        if (borrowing == null)
        {
            return NotFound();
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin = User.IsInRole("Admin");
        
        // Check authorization
        if (borrowing.UserId != userId && !isAdmin)
        {
            return Forbid();
        }

        return View(borrowing);
    }

    // Check if user can borrow a specific book (AJAX endpoint)
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> CanBorrow(int bookId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var canBorrow = await _borrowingService.CanUserBorrowBookAsync(bookId, userId);
        
        return Json(new { canBorrow });
    }

    // Get user's active borrowing count (AJAX endpoint)
    // [HttpGet]
    // [Authorize]
    // public async Task<IActionResult> GetUserActiveBorrowings()
    // {
    //     var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    //     var borrowings = await _borrowingService.GetUserBorrowingsAsync(userId);
    //     var activeBorrowings = borrowings.Where(b => !b.IsReturned).ToList();
        
    //     return Json(new { 
    //         count = activeBorrowings.Count,
    //         overdue = activeBorrowings.Count(b => b.IsOverdue),
    //         borrowings = activeBorrowings.Select(b => new {
    //             id = b.Id,
    //             bookTitle = b.Book.Title,
    //             dueDate = b.DueDate.ToString("yyyy-MM-dd"),
    //             isOverdue = b.IsOverdue,
    //             daysOverdue = b.DaysOverdue
    //         })
    //     });
    // }

    // Admin: Force return (for overdue books)
    // [HttpPost]
    // [Authorize(Roles = "Admin")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> ForceReturn(int borrowingId)
    // {
    //     var result = await _borrowingService.ReturnBookAsync(borrowingId);
        
    //     if (result)
    //     {
    //         TempData["SuccessMessage"] = "Book forcefully returned successfully!";
    //     }
    //     else
    //     {
    //         TempData["ErrorMessage"] = "Failed to return the book.";
    //     }

    //     return RedirectToAction("Index");
    // }

    // Get overdue borrowings (Admin)
    // [Authorize(Roles = "Admin")]
    // public async Task<IActionResult> Overdue()
    // {
    //     var borrowings = await _borrowingService.GetAllBorrowingsAsync();
    //     var overdueBorrowings = borrowings.Where(b => b.IsOverdue).ToList();
        
    //     return View(overdueBorrowings);
    // }

    // Get statistics (AJAX endpoint for dashboard)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetStatistics()
    {
        var borrowed = await _borrowingService.GetBorrowedCountAsync();
        var overdue = await _borrowingService.GetOverdueCountAsync();
        var returned = await _borrowingService.GetReturnedCountAsync();
        
        return Json(new { 
            borrowed, 
            overdue, 
            returned, 
            total = borrowed + returned 
        });
    }
}
