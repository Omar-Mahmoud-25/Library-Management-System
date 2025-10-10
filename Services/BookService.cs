using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services.Interfaces;
using LibraryManagementSystem.Repositories.Interfaces;

namespace LibraryManagementSystem.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly string _imagesPath;

    public BookService(IBookRepository bookRepository, IWebHostEnvironment env)
    {
        _bookRepository = bookRepository;
        _imagesPath = Path.Combine(env.WebRootPath, "images", "books");
        
        // Ensure images directory exists
        if (!Directory.Exists(_imagesPath))
        {
            Directory.CreateDirectory(_imagesPath);
        }
    }
    public Book? GetById(int id)
    {
        return _bookRepository.GetBookByIdAsync(id).Result;
    }

    public async Task<List<Book>> GetAll(int? categoryId = null, string? search = null)
    {
        var allBooks = await _bookRepository.GetAllBooksAsync();
        
        if (!categoryId.HasValue && string.IsNullOrEmpty(search))
            return allBooks;
        
        var filteredBooks = allBooks.AsQueryable();

        if (categoryId.HasValue)
        {
            filteredBooks = filteredBooks.Where(b => b.BookCategories.Any(bc => bc.CategoryId == categoryId.Value));
        }
        if (!string.IsNullOrEmpty(search))
        {
            filteredBooks = filteredBooks.Where(b => b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                                        b.Author.Contains(search, StringComparison.OrdinalIgnoreCase));
        }
        return filteredBooks.ToList();
        
    }
    public async Task Create(CreateBookViewModel model)
    {
        var book = new Book
        {
            Title = model.Title,
            Author = model.Author,
            Description = model.Description ?? string.Empty,
            PublishedDate = model.PublishedDate,
            CopiesAvailable = model.CopiesAvailable
        };

        // Handle cover image upload
        if (model.CoverImageUrl != null)
        {
            book.CoverImageUrl = await SaveCoverAsync(model.CoverImageUrl);
        }
        else
        {
            book.CoverImageUrl = "default-book-cover.png"; // Default image
        }

        // Add categories
        foreach (var categoryId in model.SelectedCategoryIds)
        {
            book.BookCategories.Add(new BookCategory
            {
                CategoryId = categoryId
            });
        }
        
        await _bookRepository.AddBookAsync(book);
    }
    public async Task<Book?> UpdateAsync(EditBookViewModel model)
    {
        var book = await _bookRepository.GetBookWithCategoriesAsync(model.Id);

        if (book == null)
            return null;

        var hasNewCover = model.CoverImageUrl != null;
        var oldCover = book.CoverImageUrl;

        // Update book properties
        book.Title = model.Title;
        book.Author = model.Author;
        book.Description = model.Description ?? string.Empty;
        book.PublishedDate = model.PublishedDate;
        book.CopiesAvailable = model.CopiesAvailable;

        // Update categories
        book.BookCategories.Clear();
        foreach (var categoryId in model.SelectedCategoryIds)
        {
            book.BookCategories.Add(new BookCategory
            {
                CategoryId = categoryId
            });
        }

        // Handle cover image
        if (hasNewCover)
        {
            book.CoverImageUrl = await SaveCoverAsync(model.CoverImageUrl!);
        }

        try
        {
            await _bookRepository.UpdateBookAsync(book);

            // Clean up old cover if update was successful and we have a new cover
            if (hasNewCover && !string.IsNullOrEmpty(oldCover) && oldCover != "default-book-cover.png")
            {
                var coverPath = Path.Combine(_imagesPath, oldCover);
                if (File.Exists(coverPath))
                    File.Delete(coverPath);
            }

            return book;
        }
        catch
        {
            // Clean up new cover if update failed
            if (hasNewCover && !string.IsNullOrEmpty(book.CoverImageUrl))
            {
                var coverPath = Path.Combine(_imagesPath, book.CoverImageUrl);
                if (File.Exists(coverPath))
                    File.Delete(coverPath);
            }

            return null;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);

        if (book == null)
            return false;

        try
        {
            await _bookRepository.DeleteBookAsync(id);

            // Clean up cover image after successful deletion (but not default image)
            if (!string.IsNullOrEmpty(book.CoverImageUrl) && book.CoverImageUrl != "default-book-cover.png")
            {
                var coverPath = Path.Combine(_imagesPath, book.CoverImageUrl);
                if (File.Exists(coverPath))
                {
                    File.Delete(coverPath);
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<string> SaveCoverAsync(IFormFile coverImage)
    {
        // Ensure the images directory exists
        if (!Directory.Exists(_imagesPath))
        {
            Directory.CreateDirectory(_imagesPath);
        }

        // Generate unique filename
        var fileExtension = Path.GetExtension(coverImage.FileName);
        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(_imagesPath, fileName);

        // Validate file type
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        if (!allowedExtensions.Contains(fileExtension.ToLowerInvariant()))
        {
            throw new InvalidOperationException("Invalid file type. Only image files are allowed.");
        }

        // Validate file size (limit to 5MB)
        if (coverImage.Length > 5 * 1024 * 1024)
        {
            throw new InvalidOperationException("File size exceeds the 5MB limit.");
        }

        // Save the file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await coverImage.CopyToAsync(stream);
        }

        return fileName; // Return only the filename, not the full path
    }

    public string GetCoverImageUrl(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return "/images/books/default-book-cover.png";
            
        return $"/images/books/{fileName}";
    }

}
