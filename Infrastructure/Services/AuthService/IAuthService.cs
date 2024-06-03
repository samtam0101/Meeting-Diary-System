using Domain.DTOs.AuthDto;
using Domain.Responses;

namespace Infrastructure.Services.AuthService;

public interface IAuthService
{
    Task<Response<string>> Login(LoginDto loginDto);
    Task<Response<string>> Register(RegisterDto registerDto);
    Task<Response<string>> ResetPassword(ResetPasswordDto resetPasswordDto);
    Task<Response<string>> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
    Task<Response<string>> ChangePassword(ChangePasswordDto changePasswordDto, int userId);
    Task<Response<string>> DeleteAccount(int id);
}
