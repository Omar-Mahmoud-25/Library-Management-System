namespace LibraryManagementSystem.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName => $"{FirstName} {LastName}";

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public DateTime JoiningDate { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }

        public int BorrowedBooksCount { get; set; }

        public string Role => IsAdmin ? "Admin" : "User";
        public string Status => IsActive ? "Active" : "Inactive";
    }
}
