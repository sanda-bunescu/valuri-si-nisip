namespace Domain.DTOs.Login;

public class LoginResponse
{
    public Guid accountId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string AccessToken { get; set; }
}