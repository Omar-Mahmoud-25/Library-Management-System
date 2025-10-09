using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Models.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }         
        public string Title { get; set; }    
        public string Author { get; set; }     
        public string Category { get; set; }   
        public string CoverImageUrl { get; set; } 
    }
}
