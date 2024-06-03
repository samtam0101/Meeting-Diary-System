namespace Domain.DTOs.NotificationDto;

public class AddNotificationDto
{
    public int UserId { get; set; }
    public int MeetingId { get; set; }
    public string Message { get; set; }
    public DateTime SentDateTime { get; set; }
}
