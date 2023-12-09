using serveu.Dtos;

public class LoginResponseDto
{
    public ApplicationUserDto User { get; set; }
    public string AccessToken { get; set; }
}