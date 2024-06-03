namespace Domain.DTOs.MeetingDto;

public class AddMeetingDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public int UserId { get; set; }
}
