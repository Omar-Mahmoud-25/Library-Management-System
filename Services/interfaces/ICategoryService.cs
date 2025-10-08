using LibraryManagementSystem.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<SelectListItem>> GetSelectList();
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<bool> AddCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
