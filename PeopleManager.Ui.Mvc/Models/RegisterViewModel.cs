using System.ComponentModel.DataAnnotations;

namespace PeopleManager.Ui.Mvc.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public required string ConfirmPassword { get; set; }
    }
}
