using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using backend.DTO;
using backend.Services;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService) {
        _accountService = accountService;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model) 
    {
        if (!ModelState.IsValid) 
        {
            return BadRequest(ModelState);
        }

        var result = await _accountService.RegisterAsync(model);

        if (result.Succeeded) 
        {
            return Ok((new { Message = "Registration was successful. Check your email to confirm your account" }));
        }

        return BadRequest(result.Errors);
    }

    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model) 
    {
        var token = await _accountService.LoginAsync(model);

        if (token != null)
        {
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }

    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return Ok(new { Message = "User logged out successfully." });
    }

    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAccount() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _accountService.DeleteAccountAsync(userId);
        
        if (result.Succeeded)
        {
            return Ok(new { Message = "User account deleted successfully." });
        }

        return BadRequest(result.Errors);
    }

    
    [HttpGet("confirmemail")]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
        {
            return BadRequest("Error confirming your email.");
        }

        var result = await _accountService.ConfirmEmailAsync(userId, code);

        if (result.Succeeded)
        {
            return Ok("Email confirmed successfully.");
        }

        return BadRequest("Error confirming your email.");
    }
}