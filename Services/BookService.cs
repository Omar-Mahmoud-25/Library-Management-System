using LibraryManagementSystem.Context;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

public class BookService : IBookService
{
    private readonly LMSContext _context;
    private readonly string _imagesPath;
    public BookService(LMSContext context, IWebHostEnvironment env)
    {
        _context = context;
        _imagesPath = Path.Combine(env.WebRootPath, "images", "books");
    }
    public Book? GetById(int id)
    {
        return _context.Books
            .Include(b => b.BookCategories)       
            .ThenInclude(bc => bc.Category) 
            .AsNoTracking()                     
            .SingleOrDefault(b => b.Id == id);   
    }
    public async Task<List<Book>> GetAll()
    {
        return await _context.Books
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task Create(CreateBookViewModel model)
    {
        var book = new Book
        {
            Title = model.Title,
            Author = model.Author,
            Description = model.Description,
            PublishedDate = model.PublishedDate,
            CopiesAvailable = model.CopiesAvailable,
            CoverImageUrl = model.CoverImageUrl
        };

        book.BookCategories.Add(new BookCategory
        {
            CategoryId = model.CategoryId
        });

        _context.Books.Add(book);
        await _context.SaveChangesAsync();
    }
    public async Task<EditBookViewModel> GetByIdForEditAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) throw new Exception("Book not found");

        return new EditBookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            CategoryId = book.CategoryId,
            Categories = _CategoryService.GetSelectList()
        };
    }
    public async Task UpdateAsync(EditBookViewModel model)
    {
        var book = await _context.Books.FindAsync(model.Id);
        if (book == null) throw new Exception("Book not found");

        book.Title = model.Title;
        book.Author = model.Author;
        book.BookCategories.Clear();
        book.BookCategories.Add(new BookCategory
        {
            BookId = book.Id,
            CategoryId = model.CategoryId
        });


        await _context.SaveChangesAsync();
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
            return false;

        _context.Books.Remove(book);
        var effectedRows = await _context.SaveChangesAsync();

        if (effectedRows > 0)
        {
            if (!string.IsNullOrEmpty(book.CoverImageUrl))
            {
                var coverPath = Path.Combine(_imagesPath, Path.GetFileName(book.CoverImageUrl));
                if (File.Exists(coverPath))
                {
                    File.Delete(coverPath);
                }
            }

            return true;
        }

        return false;
    }
}
