using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using backend.DTO;
using backend.Interfaces;
namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccount _account;

    public AccountController(IAccount account)
    {
        _account = account;
    }

    [HttpPost("/account/register")]
    public async Task<IActionResult> RegisterAccount([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _account.RegisterAccount(model);

        if (result.Succeeded)
        {
            return Ok(new { Message = "Registration was successful." });
        }

        return BadRequest(result.Errors);
    }

    [HttpDelete("/account/delete")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _account.DeleteAccount(userId);

        if (result.Succeeded)
        {
            return Ok(new { Message = "User account deleted successfully." });
        }

        return BadRequest(result.Errors);
    }

    [HttpGet("/account/confirmemail")]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
        {
            return BadRequest("Error confirming your email.");
        }

        var result = await _account.ConfirmEmail(userId, code);

        if (result.Succeeded)
        {
            return Ok("Email confirmed successfully.");
        }

        return BadRequest("Error confirming your email.");
    }
}
