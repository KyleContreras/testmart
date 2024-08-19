using Microsoft.AspNetCore.Mvc;
using backend.DTO;
using backend.Interfaces;

namespace backend.Controllers;


[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthorization _authorization;

    public AuthController(IAuthorization authorization) {
        _authorization = authorization;
    }

    
    [HttpPost("/auth/login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
    {
        var token = await _authorization.LoginAsync(model);

        if (token != null)
        {
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }


    [HttpPost("/auth/logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await _authorization.LogoutAsync();
        return Ok(new { Message = "User logged out successfully." });
    }
    
    // TODO: Generate New JWT

}