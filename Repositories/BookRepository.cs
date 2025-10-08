using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LMSContext _context;

    public BookRepository(LMSContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _context.Books
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book?> GetBookWithCategoriesAsync(int id)
    {
        return await _context.Books
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task AddBookAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(Book book)
    {
        var existingBook = await _context.Books
            .Include(b => b.BookCategories)
            .FirstOrDefaultAsync(b => b.Id == book.Id);
            
        if (existingBook != null)
        {
            // Update scalar properties
            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Description = book.Description;
            existingBook.PublishedDate = book.PublishedDate;
            existingBook.CopiesAvailable = book.CopiesAvailable;
            existingBook.CoverImageUrl = book.CoverImageUrl;
            
            // Update categories
            existingBook.BookCategories.Clear();
            foreach (var bookCategory in book.BookCategories)
            {
                existingBook.BookCategories.Add(new BookCategory 
                { 
                    CategoryId = bookCategory.CategoryId 
                });
            }
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> BookExistsAsync(int id)
    {
        return await _context.Books.AnyAsync(b => b.Id == id);
    }
}