using System.ComponentModel.DataAnnotations;
using serveu.Models;

namespace serveu.Dtos;

public class CreateUserDto
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


    public bool IsVerified { get; set; }

}

public class UpdateUserDto
{

    [Required]
    public string Id
    {
        get; set;
    }

    [Required]
    [MaxLength(32)]
    [MinLength(2)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Should be a valid email")]
    public string Email { get; set; }

    public bool IsVerified { get; set; }

}

public class CreateRestaurantDto : CreateUserDto
{

    [Required(ErrorMessage = "Phone number is required")]
    [MaxLength(64)]
    [MinLength(8)]
    public string PhoneNumber { get; set; }


    public bool IsVerified { get; set; }

}

public class UpdateRestaurantDto : UpdateUserDto
{
    [Required]
    public string Id
    {
        get; set;
    }

    [Required(ErrorMessage = "Phone number is required")]
    [MaxLength(64)]
    [MinLength(8)]
    public string PhoneNumber { get; set; }


}