using Domain.Constants;
using Domain.DTOs.NotificationDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]
public class NotificationController(INotificationService notificationService):ControllerBase
{
    [HttpGet("Get-Notifications")]
    [PermissionAuthorize(Permissions.Notifications.View)]
    public async Task<PagedResponse<List<GetNotificationDto>>> GetNotificationsAsync([FromQuery]NotificationFilter filter)
    {
        return await notificationService.GetNotificationsAsync(filter);
    }
    [HttpGet("Get-NotificationById")]
    [PermissionAuthorize(Permissions.Notifications.View)]
    public async Task<Response<GetNotificationDto>> GetNotificationByIdAsync(int id)
    {
        return await notificationService.GetNotificationByIdAsync(id);
    }
    [HttpPost("Add-Notification")]
    [PermissionAuthorize(Permissions.Notifications.Create)]
    public async Task<Response<string>> AddNotificationAsync(AddNotificationDto addNotificationDto)
    {
        return await notificationService.AddNotificationAsync(addNotificationDto);
    }
    [HttpPut("Update-Notification")]
    [PermissionAuthorize(Permissions.Notifications.Edit)]
    public async Task<Response<string>> UpdateNotificationAsync(UpdateNotificationDto updateNotificationDto)
    {
        return await notificationService.UpdateNotificationAsync(updateNotificationDto);
    }
    [HttpDelete("Delete-Notification")]
    [PermissionAuthorize(Permissions.Notifications.Delete)]
    public async Task<Response<bool>> DeleteNotificationAsync(int id)
    {
        return await notificationService.DeleteNotificationAsync(id);
    }
}
