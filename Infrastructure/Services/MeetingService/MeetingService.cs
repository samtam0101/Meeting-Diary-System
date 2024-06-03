using System.Net;
using AutoMapper;
using Domain.DTOs.MeetingDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.MeetingService;

public class MeetingService(DataContext context, IMapper mapper, ILogger<MeetingService> logger):IMeetingService
{
 
    public async Task<Response<string>> AddMeetingAsync(AddMeetingDto addMeetingDto)
    {
        try
        {
            logger.LogInformation("AddMeeting method started at {DateTime}", DateTime.Now);
            var existing = await context.Meetings.AnyAsync(e => e.UserId == addMeetingDto.UserId);
            if (existing) return new Response<string>(HttpStatusCode.BadRequest, "Meeting already exists!");
            var mapped = mapper.Map<Meeting>(addMeetingDto);
            await context.Meetings.AddAsync(mapped);
            await context.SaveChangesAsync();
            logger.LogInformation("AddMeeting method finished at {DateTime}", DateTime.Now);
            return new Response<string>("Added successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteMeetingAsync(int id)
    {
        try
        {
            logger.LogInformation("DeleteMeeting method started at {DateTime}", DateTime.Now);
            var existing = await context.Meetings.Where(e => e.Id == id).ExecuteDeleteAsync();
            if (existing == 0)
            {
                logger.LogWarning("Meeting not found {Id} at {DateTime}", id, DateTime.Now);
                return new Response<bool>(HttpStatusCode.BadRequest, "Meeting not found!");
            }
            logger.LogInformation("DeleteMeeting method finished at {DateTime}", DateTime.Now);
            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<bool>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<PagedResponse<List<GetMeetingDto>>> GetMeetingsAsync(MeetingFilter filter)
    {
        try
        {
            logger.LogInformation("GetMeeting method started at {DateTime}", DateTime.Now);
            var Meetings = context.Meetings.AsQueryable();
            if ( !string.IsNullOrEmpty(filter.Title))
            Meetings = Meetings.Where(x => x.Title.ToLower().Contains(filter.Title.ToLower()));
            if ( !string.IsNullOrEmpty(filter.Description))
            Meetings = Meetings.Where(x => x.Description.ToLower().Contains(filter.Description.ToLower()));

            var result = await Meetings.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var total = await Meetings.CountAsync();

            var response = mapper.Map<List<GetMeetingDto>>(result);
            logger.LogInformation("GetMeeting method finished at {DateTime}", DateTime.Now);
            return new PagedResponse<List<GetMeetingDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new PagedResponse<List<GetMeetingDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetMeetingDto>> GetMeetingByIdAsync(int id)
    {
        try
        {

            logger.LogInformation("GetMeetingById method started at {DateTime}", DateTime.Now);
            var existing = await context.Meetings.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) { 
                logger.LogWarning("Meeting not found {Id} at {DateTime}", id, DateTime.Now);
                return new Response<GetMeetingDto>(HttpStatusCode.BadRequest, "Meeting not found"); }
            var Meetings = mapper.Map<GetMeetingDto>(existing);
            logger.LogInformation("GetMeetingById method finished at {DateTime}", DateTime.Now);
            return new Response<GetMeetingDto>(Meetings);
        }
        catch (Exception e)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<GetMeetingDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateMeetingAsync(UpdateMeetingDto updateMeetingDto)
    {
        try
        {
            logger.LogInformation("UpdateMeeting method started at {DateTime}", DateTime.Now);
            var existing = await context.Meetings.AnyAsync(e => e.Id == updateMeetingDto.Id);
            if (!existing) return new Response<string>(HttpStatusCode.BadRequest, "Meeting not found!");
            var mapped = mapper.Map<Meeting>(updateMeetingDto);
            context.Meetings.Update(mapped);
            await context.SaveChangesAsync();
            logger.LogInformation("UpdateMeeting method finished at {DateTime}", DateTime.Now);
            return new Response<string>("Updated successfully");
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}