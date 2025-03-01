using Domain.DTOs.Login;
using Domain.DTOs.Register;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Infrastructure.Common;
using Infrastructure.Mappers;

namespace Infrastructure.Services;

public interface IAccountService
{
    Task RegisterAsync(RegisterRequest registerRequest);
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    Task<LoginResponse> GetUserByIdAsync(Guid userId);
}

public class AccountService : ServiceBase, IAccountService
{
    private readonly IJwtService _jwtService;
    public AccountService(IUnitOfWork unitOfWork, IJwtService jwtService) : base(unitOfWork)
    {
        _jwtService = jwtService;
    }

    public async Task RegisterAsync(RegisterRequest registerRequest)
    {
        if (await UnitOfWork.AccountRepository.GetByEmailAsync(registerRequest.Email) != null)
            throw new BadRequestException("Email already registered");
        
        if (!Helper.IsEmail(registerRequest.Email))
            throw new BadRequestException("Invalid email");
        
        if (!Helper.IsStrongPassword(registerRequest.Password))
            throw new BadRequestException("Weak password");

        var newAccount = new Account()
        {
            Name = registerRequest.Name,
            Email = registerRequest.Email,
            Password = PasswordHelper.GetPasswordHash(registerRequest.Password)
        };
            
        await UnitOfWork.AccountRepository.AddAsync(newAccount);
        await UnitOfWork.SaveChangesAsync();
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var user = await UnitOfWork.AccountRepository.GetByEmailAsync(loginRequest.Email);
        if (user == null)
            throw new BadRequestException("User does not exist");
        
        var result = PasswordHelper.CheckPasswordHash(loginRequest.Password, user.Password);
        if (!result)
            throw new BadRequestException("Wrong password");
        
        return _jwtService.GenerateLoginResponse(user);
    }

    public async Task<LoginResponse> GetUserByIdAsync(Guid userId)
    {
        var user = await UnitOfWork.AccountRepository.GetByIdAsync(userId);
        if (user == null)
            throw new BadRequestException("User does not exist");

        var loginResponse = user.ToLoginResponse();
        
        return loginResponse;
    }
}