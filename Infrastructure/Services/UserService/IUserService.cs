using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.UserService;

public interface IUserService
{
    Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUserDto, int userId);
    Task<PagedResponse<List<GetUserDto>>> GetUsersAsync(UserFilter filter);
    Task<Response<GetUserDto>> GetUserByIdAsync(int id);
}
