namespace Domain.DTOs.UserDto;

public class GetUserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string? Photo { get; set; }
}
