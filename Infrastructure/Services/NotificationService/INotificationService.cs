using Domain.DTOs.NotificationDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.NotificationService;

public interface INotificationService
{
    Task<Response<string>> AddNotificationAsync(AddNotificationDto addNotificationDto);
    Task<Response<string>> UpdateNotificationAsync(UpdateNotificationDto updateNotificationDto);
    Task<Response<bool>> DeleteNotificationAsync(int id );
    Task<Response<GetNotificationDto>> GetNotificationByIdAsync(int id);
    Task<PagedResponse<List<GetNotificationDto>>> GetNotificationsAsync(NotificationFilter filter);
}
