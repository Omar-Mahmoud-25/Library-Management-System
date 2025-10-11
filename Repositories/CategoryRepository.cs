using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Context;

namespace LibraryManagementSystem.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly LMSContext _context;

    public CategoryRepository(LMSContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.Include(c => c.BookCategories).ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories
            .Include(c => c.BookCategories)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> CategoryExistsAsync(string name)
    {
        return await _context.Categories.AnyAsync(c => c.Name == name);
    }
}