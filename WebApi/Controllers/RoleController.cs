using Domain.Constants;
using Domain.DTOs.RoleDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.RoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RoleController(IRoleService roleService):ControllerBase
{
    [HttpGet("Get-Roles")]
    [PermissionAuthorize(Permissions.Roles.View)]
    public async Task<PagedResponse<List<GetRoleDto>>> GetRolesAsync([FromQuery]RoleFilter filter)
    {
        return await roleService.GetRolesAsync(filter);
    }
    [HttpPost("Add-Role")]
    [PermissionAuthorize(Permissions.Roles.Create)]
    public async Task<Response<string>> AddRoleAsync(AddRoleDto addRoleDto)
    {
        return await roleService.AddRoleAsync(addRoleDto);
    }
    [HttpPut("Update-Role")]
    [PermissionAuthorize(Permissions.Roles.Edit)]
    public async Task<Response<string>> UpdateRoleAsync(UpdateRoleDto updateRoleDto)
    {
        return await roleService.UpdateRoleAsync(updateRoleDto);
    }
    [HttpGet("Get-RoleById")]
    [PermissionAuthorize(Permissions.Roles.View)]
    public async Task<Response<GetRoleDto>>GetRoleByIdAsync(int id)
    {
        return await roleService.GetRoleByIdAsync(id);
    }
    [HttpDelete("Delete-Role")]
    [PermissionAuthorize(Permissions.Roles.Delete)]
    public async Task<Response<bool>> DeleteRoleAsync(int id)
    {
        return await roleService.DeleteRoleAsync(id);
    }
}