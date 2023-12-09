using serveu.Models;

namespace serveu.Dtos;

public class ApplicationUserDto
{

    public string Id { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsVerified { get; set; }

    public string Role { get; set; }
    public string? PhoneNumber { get; set; }
    public MenuItemDTO[]? MenuItems { get; set; }


}