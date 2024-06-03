using System.Net;
using AutoMapper;
using Domain.DTOs.NotificationDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.NotificationService;

public class NotificationService(DataContext context, IMapper mapper, ILogger<NotificationService> logger):INotificationService
{
 
    public async Task<Response<string>> AddNotificationAsync(AddNotificationDto addNotificationDto)
    {
        try
        {
            logger.LogInformation("AddNotification method started at {DateTime}", DateTime.Now);
            var existing = await context.Notifications.AnyAsync(e => e.UserId == addNotificationDto.UserId);
            if (existing) return new Response<string>(HttpStatusCode.BadRequest, "Notification already exists!");
            var mapped = mapper.Map<Notification>(addNotificationDto);
            await context.Notifications.AddAsync(mapped);
            await context.SaveChangesAsync();
            logger.LogInformation("AddNotification method finished at {DateTime}", DateTime.Now);
            return new Response<string>("Added successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteNotificationAsync(int id)
    {
        try
        {
            logger.LogInformation("DeleteNotification method started at {DateTime}", DateTime.Now);
            var existing = await context.Notifications.Where(e => e.Id == id).ExecuteDeleteAsync();
            if (existing == 0)
            {
                logger.LogWarning("Notification not found {Id} at {DateTime}", id, DateTime.Now);
                return new Response<bool>(HttpStatusCode.BadRequest, "Notification not found!");
            }
            logger.LogInformation("DeleteNotification method finished at {DateTime}", DateTime.Now);
            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<bool>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<PagedResponse<List<GetNotificationDto>>> GetNotificationsAsync(NotificationFilter filter)
    {
        try
        {
            logger.LogInformation("GetNotification method started at {DateTime}", DateTime.Now);
            var notifications = context.Notifications.AsQueryable();
            if ( !string.IsNullOrEmpty(filter.Message))
            notifications = notifications.Where(x => x.Message.ToLower().Contains(filter.Message.ToLower()));

            var result = await notifications.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var total = await notifications.CountAsync();

            var response = mapper.Map<List<GetNotificationDto>>(result);
            logger.LogInformation("GetNotification method finished at {DateTime}", DateTime.Now);
            return new PagedResponse<List<GetNotificationDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new PagedResponse<List<GetNotificationDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetNotificationDto>> GetNotificationByIdAsync(int id)
    {
        try
        {

            logger.LogInformation("GetNotificationById method started at {DateTime}", DateTime.Now);
            var existing = await context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) { 
                logger.LogWarning("Notification not found {Id} at {DateTime}", id, DateTime.Now);
                return new Response<GetNotificationDto>(HttpStatusCode.BadRequest, "Notification not found"); }
            var Notifications = mapper.Map<GetNotificationDto>(existing);
            logger.LogInformation("GetNotificationById method finished at {DateTime}", DateTime.Now);
            return new Response<GetNotificationDto>(Notifications);
        }
        catch (Exception e)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<GetNotificationDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateNotificationAsync(UpdateNotificationDto updateNotificationDto)
    {
        try
        {
            logger.LogInformation("UpdateNotification method started at {DateTime}", DateTime.Now);
            var existing = await context.Notifications.AnyAsync(e => e.Id == updateNotificationDto.Id);
            if (!existing) return new Response<string>(HttpStatusCode.BadRequest, "Notification not found!");
            var mapped = mapper.Map<Notification>(updateNotificationDto);
            context.Notifications.Update(mapped);
            await context.SaveChangesAsync();
            logger.LogInformation("UpdateNotification method finished at {DateTime}", DateTime.Now);
            return new Response<string>("Updated successfully");
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
