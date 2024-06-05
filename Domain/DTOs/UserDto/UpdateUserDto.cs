using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.UserDto;

public class UpdateUserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
}
