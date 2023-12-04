using System.ComponentModel.DataAnnotations;

namespace PeopleManager.Dto.Requests
{
    public class PersonRequest
    {
        [Display(Name = "First Name")]
        [Required]
        public required string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public required string LastName { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}
