using Domain.Constants;
using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController(IUserService userService):ControllerBase
{
    [HttpGet("Get-Users")]
    [PermissionAuthorize(Permissions.Users.View)]
    public async Task<PagedResponse<List<GetUserDto>>> GetUsersAsync([FromQuery] UserFilter filter)
    {
        return await userService.GetUsersAsync(filter);
    }

    [HttpGet("Get-UserById")]
    [PermissionAuthorize(Permissions.Users.View)]
    public async Task<Response<GetUserDto>> GetUserById(int userId)
    {
        return await userService.GetUserByIdAsync(userId);
    }
    [HttpPut("Update-User")]
    [PermissionAuthorize(Permissions.Users.Edit)]
    public async Task<Response<string>> UpdateUserAsync([FromBody]UpdateUserDto updateUserDto)
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "sid")?.Value);
        return await userService.UpdateUserAsync(updateUserDto, userId);
    }
}
