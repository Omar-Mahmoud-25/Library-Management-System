using Microsoft.AspNetCore.Mvc.Rendering;

public class EditBookViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public DateTime PublishedDate { get; set; }
    public int CopiesAvailable { get; set; }
    public string CoverImageUrl { get; set; }

    public int SelectedCategoryId { get; set; }
    public List<SelectListItem> Categories { get; set; } = new();
}
