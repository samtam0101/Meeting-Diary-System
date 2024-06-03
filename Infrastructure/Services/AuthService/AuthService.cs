using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.DTOs.AuthDto;
using Domain.DTOs.EmailDto;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.EmailService;
using Infrastructure.Services.FileService;
using Infrastructure.Services.HashService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MimeKit.Text;

namespace Infrastructure.Services.AuthService;

public class AuthService(DataContext context, ILogger<AuthService> logger, IHashService hashService, IFileService fileService, IConfiguration configuration, IEmailService emailService) : IAuthService
{
    public async Task<Response<string>> ChangePassword(ChangePasswordDto changePasswordDto, int userId)
    {
        try
        {
            logger.LogInformation("Method Changepassword started at {DateTime}", DateTime.UtcNow);

            var existing = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (existing is null)
            {
                logger.LogWarning("User not found,in time {DateTime}", DateTimeOffset.Now);
                return new Response<string>(HttpStatusCode.BadRequest, "User not found");
            }
            if (existing.Password != hashService.ConvertToHash(changePasswordDto.OldPassword))
            {
                logger.LogWarning("Failed old Password,in time {DateTime}", DateTimeOffset.Now);
                return new Response<string>(HttpStatusCode.BadRequest, "Failed old Password");
            }
            existing.Password = hashService.ConvertToHash(changePasswordDto.NewPassword);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {ChangePassword} in time {DateTime}", "ChangePassword",
                    DateTimeOffset.UtcNow);
            return new Response<string>("Successfully changed password");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception} in time:{DateTime} ", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> DeleteAccount(int userId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteAccount} in time:{DateTime} ", "DeleteAccount",
                DateTimeOffset.UtcNow);

            var user = await context.Users.Where(x => x.Id == userId).ExecuteDeleteAsync();
            logger.LogInformation("Finished method {DeleteAccount} in time:{DateTime} ", "DeleteAccount",
                DateTimeOffset.UtcNow);
            return user == 0
                ? new Response<string>(HttpStatusCode.BadRequest, $"User not found by Id:{userId}")
                : new Response<string>("Successfully deleted account");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            logger.LogInformation("Starting method {ForgotPasswordTokenGenerator} in time {DateTime}", "Register",
                DateTimeOffset.UtcNow);
            var existing = await context.Users.FirstOrDefaultAsync(x => x.Email == forgotPasswordDto.Email);
            if (existing is null)
            {
                logger.LogWarning("Not found user with email {Email} ,time={DateTimeNow} ", forgotPasswordDto.Email,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest,
                    $"Not Found User with email {forgotPasswordDto.Email}");
            }

            var random = new Random();

            existing.Code = random.Next(1000, 10000).ToString();
            existing.CodeTime = DateTime.UtcNow;

            await context.SaveChangesAsync();


            await emailService.SendEmail(new EmailMessageDto(new[] { forgotPasswordDto.Email }, "reset password",
                $"<h1>{existing.Code}</h1>"), TextFormat.Html);


            logger.LogInformation("Finished method {ForgotPasswordTokenGenerator} in time {DateTime}", "Register",
                DateTimeOffset.UtcNow);
            return new Response<string>("Success");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception} in time:{DateTime} ", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> Login(LoginDto loginDto)
    {
        try
        {
            logger.LogInformation("Login method started at {DateTime}", DateTime.UtcNow);
            var existing = await context.Users.FirstOrDefaultAsync(x => x.Name == loginDto.UserName && x.Password == hashService.ConvertToHash(loginDto.Password));
            if (existing is null)
            {
                logger.LogWarning("Username or password incorrect,time={DateTimeNow} ", DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "UserName or Password is incorrect");
            }
            logger.LogInformation("Finished method {Login} in time {DateTime}", "Login", DateTimeOffset.UtcNow);
            return new Response<string>(await GenerateJwtToken(existing));
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception} in time:{DateTime} ", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> Register(RegisterDto registerDto)
    {
        try
        {
            logger.LogInformation("Starting method {Register} in time {DateTime}", "Register", DateTimeOffset.UtcNow);
            var existing = await context.Users.AnyAsync(x => x.Name == registerDto.UserName);
            if (existing)
            {
                logger.LogWarning("Already exists this {UserName}, Time:{DateTime}", registerDto.UserName, DateTime.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, $"User already exists by this username = {registerDto.UserName}");
            }
            var user = new User()
            {
                Name = registerDto.UserName,
                Email = registerDto.Email,
                Password = hashService.ConvertToHash(registerDto.Password),
                RegistrationDate = DateTime.UtcNow,
                //Photo = registerDto.Photo == null ? "null" : await fileService.CreateFile(registerDto.Photo)
            };
            var res = await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var userRole = new UserRole()
            {
                UserId = res.Entity.Id,
                RoleId = 2
            };
            await context.UserRoles.AddAsync(userRole);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method Register, Time:{DateTime}", DateTime.UtcNow);
            return new Response<string>($"Successfully craeted new user by id:{user.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception} in time:{DateTime} ", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            logger.LogInformation("Starting method {ResetPassword} in time {DateTime}", "ResetPassword",
                DateTimeOffset.UtcNow);
            var existing = await context.Users.FirstOrDefaultAsync(x => x.Email == resetPasswordDto.Email);
            if (existing is null)
            {
                logger.LogWarning("Not found user with {Email},time={DateTimeNow}", resetPasswordDto.Email,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, $"Not Found User with {resetPasswordDto.Email}");
            }

            var timeCode = DateTimeOffset.UtcNow - existing.CodeTime;
            var year = DateTimeOffset.UtcNow.Year - existing.CodeTime.Year;
            if (timeCode.Days == 0 && timeCode.Hours == 0 && timeCode.Minutes <= 2 && year == 0)
            {
                existing.Password = hashService.ConvertToHash(resetPasswordDto.Password);
                await context.SaveChangesAsync();

                logger.LogInformation("Finished method {ResetPassword} in time {DateTime}", "ResetPassword",
                    DateTimeOffset.UtcNow);
                return new Response<string>("Success");
            }

            logger.LogWarning("Failed {Code},time={DateTimeNow}", resetPasswordDto.Code, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.BadRequest, $"Failed {resetPasswordDto.Code}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception} in time:{DateTime} ", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
    private async Task<string> GenerateJwtToken(User user)
    {
        try
        {
            logger.LogInformation("Starting method {GenerateJwtToken} in time {DateTime}", "GenerateJwtToken",
                DateTimeOffset.UtcNow);
            var key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Name, user.Name),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
            };
            //add roles
            var roles = await context.UserRoles.Where(x => x.UserId == user.Id).Include(x => x.Role)
                .Select(x => x.Role).ToListAsync();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role!.Name));
                var roleClaims = await context.RoleClaims.Where(x => x.RoleId == role.Id).ToListAsync();
                foreach (var roleClaim in roleClaims)
                {
                    claims.Add(new Claim("Permissions", roleClaim.ClaimValue));
                }
            }
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var securityTokenHandler = new JwtSecurityTokenHandler();
            var tokenString = securityTokenHandler.WriteToken(token);
            logger.LogInformation("Finished method {GenerateJwtToken} in time {DateTime}", "GenerateJwtToken",
                DateTimeOffset.UtcNow);
            return tokenString;
        }
        catch (Exception e)
        {
            logger.LogError("Error  {Exception} in time {DateTime}", e.Message,
                DateTimeOffset.UtcNow);
            return string.Empty;
        }
    }
}
