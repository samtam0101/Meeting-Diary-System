using Domain.DTOs.RoleDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.RoleService;

public interface IRoleService
{
    Task<Response<string>> AddRoleAsync(AddRoleDto addRoleDto);
    Task<Response<string>> UpdateRoleAsync(UpdateRoleDto updateRoleDto);
    Task<Response<bool>> DeleteRoleAsync(int id );
    Task<Response<GetRoleDto>> GetRoleByIdAsync(int id);
    Task<PagedResponse<List<GetRoleDto>>> GetRolesAsync(RoleFilter filter);
}
