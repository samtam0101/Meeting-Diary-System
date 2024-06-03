namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string? Code { get; set; }
    public DateTimeOffset CodeTime { get; set; }
    public string? Photo { get; set; }
    public List<Meeting>? Meetings { get; set; }
    public List<UserRole>? UserRoles { get; set; }
    public List<Notification>? Notifications { get; set; }
}
