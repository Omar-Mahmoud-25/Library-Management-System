namespace LibraryManagementSystem.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; }

    [Required, MaxLength(50)]
    public string LastName { get; set; }

    [Required, MaxLength(100)]
    public string Email { get; set; }

    [Required, MaxLength(15)]
    public string PhoneNumber { get; set; }

    [Required]
    public DateTime JoiningDate { get; set; } = DateTime.Now;

    [Required]
    public bool IsAdmin { get; set; } = false;

    public bool IsActive { get; set; } = true;

    public virtual ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}