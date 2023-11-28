using serveu.Dtos;

public class LoginResponseDto
{
    public ApplicationUserDto user { get; set; }
    public string authToken { get; set; }
}