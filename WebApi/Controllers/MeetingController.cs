using Domain.Constants;
using Domain.DTOs.MeetingDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.MeetingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class MeetingController(IMeetingService meetingService):ControllerBase
{
    [HttpGet("Get-Meetings")]
    [PermissionAuthorize(Permissions.Meetings.View)]
    public async Task<PagedResponse<List<GetMeetingDto>>> GetMeetingsAsync([FromQuery]MeetingFilter filter)
    {
        return await meetingService.GetMeetingsAsync(filter);
    }
    [HttpGet("Get-MeetingById")]
    [PermissionAuthorize(Permissions.Meetings.View)]
    public async Task<Response<GetMeetingDto>> GetMeetingByIdAsync(int id)
    {
        return await meetingService.GetMeetingByIdAsync(id);
    }
    [HttpPost("Add-Meeting")]
    [PermissionAuthorize(Permissions.Meetings.Create)]
    public async Task<Response<string>> AddMeetingAsync(AddMeetingDto addMeetingDto)
    {
        return await meetingService.AddMeetingAsync(addMeetingDto);
    }
    [HttpPut("Update-Meeting")]
    [PermissionAuthorize(Permissions.Meetings.Edit)]
    public async Task<Response<string>> UpdateMeetingAsync(UpdateMeetingDto updateMeetingDto)
    {
        return await meetingService.UpdateMeetingAsync(updateMeetingDto);
    }
    [HttpDelete("Delete-Meeting")]
    [PermissionAuthorize(Permissions.Meetings.Delete)]
    public async Task<Response<bool>> DeleteMeetingAsync(int id)
    {
        return await meetingService.DeleteMeetingAsync(id);
    }
}