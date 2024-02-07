using System.ComponentModel.DataAnnotations;

namespace NTR.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public required string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public required string Surname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,100}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }

        public string? Specialization { get; set; }
    }
}
