using System.ComponentModel.DataAnnotations;
using System.Drawing;
using serveu.Models;

public class SignUpDto
{

    [Required]
    [MaxLength(32)]
    [MinLength(2)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Should be a valid email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MaxLength(64)]
    [MinLength(8)]
    public string Password { get; set; }


    [Required]
    [ValidValuesAttribute(UserRole.ADMIN, UserRole.RESTAURANT)]
    public string role { get; set; }
}