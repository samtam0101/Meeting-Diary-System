namespace Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public int MeetingId { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; }
    public DateTime SentDateTime { get; set; }
    public Meeting? Meeting { get; set; }
    public User? User { get; set; }
}