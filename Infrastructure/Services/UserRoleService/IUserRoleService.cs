using Domain.DTOs.UserRoleDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.UserRoleService;

public interface IUserRoleService
{
    Task<Response<string>> AddUserRoleAsync(AddUserRoleDto addUserRoleDto);
    Task<Response<string>> UpdateUserRoleAsync(UpdateUserRoleDto updateUserRoleDto);
    Task<Response<bool>> DeleteUserRoleAsync(int id );
    Task<Response<GetUserRoleDto>> GetUserRoleByIdAsync(int id);
    Task<PagedResponse<List<GetUserRoleDto>>> GetUserRolesAsync(PaginationFilter filter);
}
