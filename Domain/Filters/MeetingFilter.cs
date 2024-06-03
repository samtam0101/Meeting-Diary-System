namespace Domain.Filters;

public class MeetingFilter:PaginationFilter
{
    public string? Title { get; set; }
    public string? Description { get; set; }
}
