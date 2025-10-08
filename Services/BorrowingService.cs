using LibraryManagementSystem.Context;
using LibraryManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class BorrowingService : IBorrowingService
{
    private readonly LMSContext _context;

    public BorrowingService(LMSContext context)
    {
        _context = context;
    }

    public List<Borrowing> GetAllBorrowings()
    {
        return _context.Borrowings
            .Include(b => b.User)
            .Include(b => b.Book)
            .AsNoTracking()
            .ToList();
    }
    public int GetBorrowedCount()
    {
        return _context.Borrowings.Count(b => b.ReturnedDate == null);
    }

    public int GetOverdueCount()
    {
        return _context.Borrowings.Count(b => b.ReturnedDate == null && DateTime.Now > b.DueDate);
    }

    public int GetReturnedCount()
    {
        return _context.Borrowings.Count(b => b.ReturnedDate != null);
    }

}
