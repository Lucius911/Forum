using System.ComponentModel.DataAnnotations;

namespace Forum.DTOs
{
  public class RegisterModel
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 chars long")]
    public string Password { get; set; } = string.Empty;
    [Required]
    [Compare("Password", ErrorMessage = "Password and confirm password don't match")]
    public string ConfirmPassword { get; set; } = string.Empty;
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string DisplayName { get; set; }
  }
}
