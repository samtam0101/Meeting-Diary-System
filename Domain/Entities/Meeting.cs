namespace Domain.Entities;

public class Meeting
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public List<Notification>? Notifications { get; set; }
}