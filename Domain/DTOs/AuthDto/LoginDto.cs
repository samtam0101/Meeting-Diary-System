using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.AuthDto;

public class LoginDto
{
    public string UserName { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
