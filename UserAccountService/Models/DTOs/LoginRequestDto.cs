namespace UserAccountService.Models.DTOs;

public class LoginRequestDto
{
    public string TagOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}