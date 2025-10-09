using LibraryManagementSystem.Context;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Services.Interfaces;

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
            Category = "some category",
            CoverImageUrl = "some path"
        };

        book.BookCategories.Add(new BookCategory
        {
            CategoryId = model.CategoryId
        });

        // var coverPath = Path.Combine(_imagesPath, Path.GetFileName(model.CoverImageUrl));
        // if (File.Exists(coverPath))
        //     File.Delete(coverPath);
        // book.CoverImageUrl = await SaveCoverAsync(model.CoverImageUrl.PhysicalPath);
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
    }
    public async Task<Book?> UpdateAsync(EditBookViewModel model)
    {
        var book = await _context.Books
            .Include(b => b.BookCategories)
            .FirstOrDefaultAsync(b => b.Id == model.Id);

        if (book == null)
            return null;

        var hasNewCover = !string.IsNullOrEmpty(model.CoverImageUrl);
        var oldCover = book.CoverImageUrl;

        
        book.Title = model.Title;
        book.Author = model.Author;
        book.Description = model.Description;
        book.PublishedDate = model.PublishedDate;
        book.CopiesAvailable = model.CopiesAvailable;

        
        book.BookCategories.Clear();
        book.BookCategories.Add(new BookCategory { CategoryId = model.SelectedCategoryId });

        
        if (hasNewCover)
        {
            book.CoverImageUrl = await SaveCoverAsync(model.CoverImageUrl);
        }

        var effectedRows = await _context.SaveChangesAsync();

        
        if (effectedRows > 0)
        {
            if (hasNewCover && oldCover != null)
            {
                var coverPath = Path.Combine(_imagesPath, oldCover);
                if (File.Exists(coverPath))
                    File.Delete(coverPath);
            }

            return book;
        }
        else
        {
            
            if (hasNewCover)
            {
                var coverPath = Path.Combine(_imagesPath, book.CoverImageUrl);
                if (File.Exists(coverPath))
                    File.Delete(coverPath);
            }

            return null;
        }
    }

    
    private async Task<string> SaveCoverAsync(string coverUrl)
    {
        
        return coverUrl;
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
