using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.DTOs.Login;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;
public interface IJwtService
{
    LoginResponse GenerateLoginResponse(Account account);
    Task<bool> CheckAccessTokenAsync(Guid userId);
}
public class JwtService : ServiceBase, IJwtService
{
    private readonly IConfiguration _configuration;
    
    public JwtService(IConfiguration configuration, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _configuration = configuration;
    }

    public LoginResponse GenerateLoginResponse(Account account)
    {
        var accessToken = GenerateToken(account.Email, account.Id);
        if (string.IsNullOrEmpty(accessToken))
            throw new Exception("Access token not provided");

        var response = new LoginResponse
        {
            accountId = account.Id,
            Name = account.Name,   
            Email = account.Email,
            AccessToken = accessToken
        };
            
        return response;
    }
    
    public async Task<bool> CheckAccessTokenAsync(Guid userId)
    {
        var user = await UnitOfWork.AccountRepository.GetByIdAsync(userId);
        if (user is null)
            return false;

        return true;
    }
    
    private string GenerateToken(string username, Guid accountId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var keyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]!));
        var creds = new SigningCredentials(keyBytes, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim("accountId", accountId.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        try
        {
            return tokenHandler.WriteToken(token);
        }
        catch (Exception)
        {
            return null;
        }
    }
}