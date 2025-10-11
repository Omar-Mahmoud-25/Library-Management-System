namespace LibraryManagementSystem.Repositories;

using LibraryManagementSystem.Context;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class BorrowingRepository : IBorrowingRepository
{
    private readonly LMSContext _context;

    public BorrowingRepository(LMSContext context)
    {
        _context = context;
    }

    public async Task<List<Borrowing>> GetAllBorrowingsAsync()
    {
        return await _context.Borrowings
            .Include(b => b.User)
            .Include(b => b.Book)
            .ToListAsync();
    }

    public async Task<Borrowing?> GetBorrowingByBookAndUserAsync(int bookId, int userId)
    {
        return await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.BookId == bookId && b.UserId == userId && b.ReturnedDate == null);
    }

    public async Task<bool> BorrowBookAsync(int bookId, int userId)
    {
        var book = await _context.Books.FindAsync(bookId);
        if (book == null || book.CopiesAvailable <= 0)
            return false;

        var borrowing = new Borrowing
        {
            BookId = bookId,
            UserId = userId,
            BorrowedDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(14)
        };

        book.CopiesAvailable--;
        _context.Borrowings.Add(borrowing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ReturnBookAsync(int bookId, int userId)
    {
        var borrowing = await _context.Borrowings
            .Include(b => b.Book)
            .FirstOrDefaultAsync(b => b.BookId == bookId && b.UserId == userId && b.ReturnedDate == null);
        if (borrowing == null)
            return false;
        borrowing.ReturnedDate = DateTime.Now;
        borrowing.Book.CopiesAvailable++;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Borrowing>> GetUserBorrowingsAsync(int userId)
    {
        return await _context.Borrowings
            .Include(b => b.Book)
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> IsUserBorrowingBookAsync(int bookId, int userId)
    {
        return await _context.Borrowings.AnyAsync(b => b.BookId == bookId && b.UserId == userId && b.ReturnedDate == null);
    }

    public async Task<bool> CanUserBorrowBookAsync(int bookId, int userId)
    {
        if (await IsUserBorrowingBookAsync(bookId, userId))
            return false;
        var activeBorrowings = await _context.Borrowings
            .Where(b => b.UserId == userId && b.ReturnedDate == null)
            .CountAsync();

        var book = await _context.Books.FindAsync(bookId);
        if (book == null || book.CopiesAvailable <= 0)
            return false;

        // Assuming a user can borrow up to 5 books at a time
        return activeBorrowings < 5;
    }
    public async Task<Borrowing?> GetBorrowingByIdAsync(int borrowingId)
    {
        return await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == borrowingId);
    }
    public async Task<List<Borrowing>> GetBookBorrowingsAsync(int bookId)
    {
        return await _context.Borrowings
            .Include(b => b.User)
            .Where(b => b.BookId == bookId)
            .ToListAsync();
    }

    public async Task<int> GetBorrowedCountAsync()
    {
        return await _context.Borrowings.CountAsync(b => b.ReturnedDate == null);
    }

    public async Task<int> GetOverdueCountAsync()
    {
        return await _context.Borrowings.CountAsync(b => b.ReturnedDate == null && DateTime.Now > b.DueDate);
    }

    public async Task<int> GetReturnedCountAsync()
    {
        return await _context.Borrowings.CountAsync(b => b.ReturnedDate != null);
    }
}