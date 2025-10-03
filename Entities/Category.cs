using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Entities;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(200)]
    public string Description { get; set; }

    public virtual ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
}