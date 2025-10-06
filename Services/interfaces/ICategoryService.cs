using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface ICategoryService
    {
        List<SelectListItem> GetSelectList();
    }
}
