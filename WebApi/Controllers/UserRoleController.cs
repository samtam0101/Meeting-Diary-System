using Domain.Constants;
using Domain.DTOs.UserRoleDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.UserRoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserRoleController(IUserRoleService userRoleService):ControllerBase
{
    [HttpGet("Get-UserRoles")]
    [PermissionAuthorize(Permissions.UserRoles.View)]
    public async Task<PagedResponse<List<GetUserRoleDto>>> GetUserRolesAsync([FromQuery]PaginationFilter filter)
    {
        return await userRoleService.GetUserRolesAsync(filter);
    }
    [HttpPost("Create-UserRole")]
    [PermissionAuthorize(Permissions.UserRoles.Create)]
    public async Task<Response<string>> AddUserRoleAsync(AddUserRoleDto addUserRoleDto)
    {
        return await userRoleService.AddUserRoleAsync(addUserRoleDto);
    }
    [HttpPut("Update-UserRole")]
    [PermissionAuthorize(Permissions.UserRoles.Edit)]
    public async Task<Response<string>> UpdateUserRoleAsync(UpdateUserRoleDto updateUserRoleDto)
    {
        return await userRoleService.UpdateUserRoleAsync(updateUserRoleDto);
    }
    [HttpDelete("Delete-UserRole")]
    [PermissionAuthorize(Permissions.UserRoles.Delete)]
    public async Task<Response<bool>> DeleteUserRoleAsync(int id)
    {
        return await userRoleService.DeleteUserRoleAsync(id);
    }
    [HttpGet("Get-UserRoleById")]
    [PermissionAuthorize(Permissions.UserRoles.View)]
    public async Task<Response<GetUserRoleDto>> GetUserRoleByIdAsync(int id)
    {
        return await userRoleService.GetUserRoleByIdAsync(id);
    }
}