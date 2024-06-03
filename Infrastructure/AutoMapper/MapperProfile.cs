using AutoMapper;
using Domain.DTOs.MeetingDto;
using Domain.DTOs.NotificationDto;
using Domain.DTOs.RoleDto;
using Domain.DTOs.UserRoleDto;
using Domain.Entities;

namespace Infrastructure.AutoMapper;

public class MapperProfile:Profile
{
    public MapperProfile()
    {

        CreateMap<UserRole, AddUserRoleDto>().ReverseMap();
        CreateMap<UserRole, UpdateUserRoleDto>().ReverseMap();
        CreateMap<UserRole, GetUserRoleDto>().ReverseMap();

        CreateMap<Notification, AddNotificationDto>().ReverseMap();
        CreateMap<Notification, UpdateNotificationDto>().ReverseMap();
        CreateMap<Notification, GetNotificationDto>().ReverseMap();

        CreateMap<Meeting, AddMeetingDto>().ReverseMap();
        CreateMap<Meeting, UpdateMeetingDto>().ReverseMap();
        CreateMap<Meeting, GetMeetingDto>().ReverseMap();
    }
}
