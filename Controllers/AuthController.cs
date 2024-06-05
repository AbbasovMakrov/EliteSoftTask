using System.Security.Claims;
using EliteSoftTask.Data.DTOs;
using EliteSoftTask.Http.Requests;
using EliteSoftTask.Http.Responses;
using EliteSoftTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteSoftTask.Controllers;
[Route("/api/auth")]
[ProducesResponseType<Dictionary<string,string[]>>(400)]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;

    public AuthController(AuthService service)
    {
        _service = service;
    }
    [HttpPost("register")]
    [ProducesResponseType<AuthResponse>(201)]
    [ProducesResponseType<string>(401)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _service.Register(request);
        if (result.IsSuccess)
        {
            return Created(string.Empty,result.Success);
        }

        return Unauthorized(result.Fail.Message);
    }
    [HttpPost("login")]
    [ProducesResponseType<AuthResponse>(200)]
    [ProducesResponseType<string>(401)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _service.Login(request);
        if (result.IsSuccess)
        {
            return Ok(result.Success);
        }

        return Unauthorized(result.Fail.Message);
    }
    [Authorize]
    [HttpGet("token-status")]
    [ProducesResponseType<ClaimDTO[]>(200)]
    [ProducesResponseType<string>(401)]
    public async Task<IActionResult> TokenStatus()
    {
        return Ok(HttpContext.User.Claims.Select(c => new ClaimDTO(c.Type,c.Value)));
    }
    
}