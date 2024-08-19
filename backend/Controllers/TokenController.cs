using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class TokenController : ControllerBase
{
    private readonly IToken _tokenService;

    public TokenController(IToken tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("generate")]
    public IActionResult GenerateToken([FromBody] ApplicationUser user)
    {
        // TODO
        return Ok();
    }
    /*
    [HttpPost("refresh")]
    public IActionResult RefreshToken([FromBody] ApplicationUser user, [FromBody] string token)
    {
        // TODO
        return Ok();
    }

    [HttpPost("blacklist")]
    public IActionResult BlacklistToken([FromBody] ApplicationUser user, [FromBody] string token)
    {
        // TODO
        return Ok();
    }
    */
}