using System.ComponentModel.DataAnnotations;

public class LoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Should be a valid email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MaxLength(64)]
    [MinLength(8)]
    public string? Password { get; set; }
}