using Domain.DTOs.MeetingDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.MeetingService;

public interface IMeetingService
{
    Task<Response<string>> AddMeetingAsync(AddMeetingDto addMeetingDto);
    Task<Response<string>> UpdateMeetingAsync(UpdateMeetingDto updateMeetingDto);
    Task<Response<bool>> DeleteMeetingAsync(int id );
    Task<Response<GetMeetingDto>> GetMeetingByIdAsync(int id);
    Task<PagedResponse<List<GetMeetingDto>>> GetMeetingsAsync(MeetingFilter filter);
    //GetUpcomingMeetings
}
