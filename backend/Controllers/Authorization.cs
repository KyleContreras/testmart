using Microsoft.AspNetCore.Mvc;
using backend.DTO;
using backend.Interfaces;

namespace backend.Controllers;


[ApiController]
[Route("[controller]")]
public class Authorization : ControllerBase
{
    private readonly IAuthorization _authorization;

    public Authorization(IAuthorization authorization) {
        _authorization = authorization;
    }

    
    [HttpPost("/auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var (jwt, refreshToken) = await _authorization.Login(model);

        if (jwt != null && refreshToken != null)
        {
            return Ok(new { JWT = jwt, RefreshToken = refreshToken });
        }

        return Unauthorized();
    }


    [HttpPost("/auth/logout")]
    public async Task<IActionResult> Logout()
    {
        await _authorization.Logout();
        return Ok(new { Message = "User logged out successfully." });
    }
    
    // TODO: Generate New JWT

}