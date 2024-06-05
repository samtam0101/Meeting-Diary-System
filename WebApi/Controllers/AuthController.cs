using Domain.DTOs.AuthDto;
using Domain.Responses;
using Infrastructure.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService):ControllerBase
{
    [HttpPost("Register")]
    public async Task<Response<string>> Register([FromBody]RegisterDto registerDto)
    {
        return await authService.Register(registerDto);
    }
    [HttpPost("Login")]
    public async Task<Response<string>> Login([FromBody]LoginDto loginDto)
    {
        return await authService.Login(loginDto);
    }
    [HttpPut("Change-Password")]
    public async Task<Response<string>> ChangePassword([FromBody]ChangePasswordDto changePasswordDto)
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "sid")?.Value);
        return await authService.ChangePassword(changePasswordDto, userId);
    }
    [HttpDelete("Forgot-Password")]
    public async Task<Response<string>> ForgotPassword([FromBody]ForgotPasswordDto forgotPasswordDto)
    {
        return await authService.ForgotPassword(forgotPasswordDto);
    }
    [HttpDelete("Reset-Password")]
    public async Task<Response<string>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        return await authService.ResetPassword(resetPasswordDto);
    }
    [HttpDelete("Delete-Account")]
    public async Task<Response<string>> DeleteAccount()
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "sid")?.Value);
        return await authService.DeleteAccount(userId);
    }
}
