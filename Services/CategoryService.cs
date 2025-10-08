using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagementSystem.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> AddCategoryAsync(Category category)
    {
        try
        {
            if (await _categoryRepository.CategoryExistsAsync(category.Name))
                return false; // Category with this name already exists

            await _categoryRepository.AddCategoryAsync(category);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        try
        {
            await _categoryRepository.DeleteCategoryAsync(id);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepository.GetAllCategoriesAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _categoryRepository.GetCategoryByIdAsync(id);
    }

    public async Task<List<SelectListItem>> GetSelectList()
    {
        var categories = await GetAllCategoriesAsync();
        return categories
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .OrderBy(c => c.Text)
            .ToList();
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        try
        {
            // Check if category exists
            var existingCategory = await _categoryRepository.GetCategoryByIdAsync(category.Id);
            if (existingCategory == null)
            {
                return false;
            }

            if (await _categoryRepository.CategoryExistsAsync(category.Name))
                return false;

            await _categoryRepository.UpdateCategoryAsync(category);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

