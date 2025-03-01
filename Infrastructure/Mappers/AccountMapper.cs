using Domain.DTOs.Login;
using Domain.Entities;

namespace Infrastructure.Mappers;

public static class AccountMapper
{
    public static LoginResponse ToLoginResponse(this Account account)
    {
        var loginResponse = new LoginResponse
        {
            accountId = account.Id,
            Name = account.Name,   
            Email = account.Email,
            AccessToken = null
        };

        return loginResponse;
    }
}