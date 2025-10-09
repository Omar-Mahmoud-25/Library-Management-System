using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;

public class UserProfileViewModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; }
    
    [Required]
    [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
    [Phone(ErrorMessage = "Please enter a valid phone number")]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
    
    [Display(Name = "Member Since")]
    public DateTime JoiningDate { get; set; }
    
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
    
    [Display(Name = "Full Name")]
    public string FullName => $"{FirstName} {LastName}";
    
    public int TotalBorrowings { get; set; }
    public int ActiveBorrowings { get; set; }
}