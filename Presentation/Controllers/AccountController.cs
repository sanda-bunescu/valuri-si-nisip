using Domain.DTOs.Login;
using Domain.DTOs.Register;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : GenericController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync([FromBody] RegisterRequest registerRequest)
    {
        await _accountService.RegisterAsync(registerRequest);
        
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<ActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
    {
        var result = await _accountService.LoginAsync(loginRequest);

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult> GetCurrentUser()
    {
        var result = await _accountService.GetUserByIdAsync(AuthInfo.UserId);
        
        return Ok(result);
    }
}