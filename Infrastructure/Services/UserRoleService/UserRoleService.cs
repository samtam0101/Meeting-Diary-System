using System.Net;
using AutoMapper;
using Domain.DTOs.UserRoleDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.UserRoleService;

public class UserRoleService(IMapper mapper, ILogger<UserRoleService> logger, DataContext context):IUserRoleService
{
    public async Task<Response<string>> AddUserRoleAsync(AddUserRoleDto addUserRoleDto)
    {
        try
        {
            logger.LogInformation("AddUserRole method started at {DateTime}", DateTime.Now);
            var existing = await context.UserRoles.AnyAsync(e => e.UserId == addUserRoleDto.UserId);
            if (existing) return new Response<string>(HttpStatusCode.BadRequest, "UserRole already exists!");
            var mapped = mapper.Map<UserRole>(addUserRoleDto);
            await context.UserRoles.AddAsync(mapped);
            await context.SaveChangesAsync();
            logger.LogInformation("AddUserRole method finished at {DateTime}", DateTime.Now);
            return new Response<string>("Added successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteUserRoleAsync(int id)
    {
        try
        {
            logger.LogInformation("DeleteUserRole method started at {DateTime}", DateTime.Now);
            var existing = await context.UserRoles.Where(e => e.Id == id).ExecuteDeleteAsync();
            if (existing == 0)
            {
                logger.LogWarning("UserRole not found {Id} at {DateTime}", id, DateTime.Now);
                return new Response<bool>(HttpStatusCode.BadRequest, "UserRole not found!");
            }
            logger.LogInformation("DeleteUserRole method finished at {DateTime}", DateTime.Now);
            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<bool>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<PagedResponse<List<GetUserRoleDto>>> GetUserRolesAsync(PaginationFilter filter)
    {
        try
        {
            logger.LogInformation("GetUserRole method started at {DateTime}", DateTime.Now);
            var UserRoles = context.UserRoles.AsQueryable();
            var result = await UserRoles.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var total = await UserRoles.CountAsync();

            var response = mapper.Map<List<GetUserRoleDto>>(result);
            logger.LogInformation("GetUserRole method finished at {DateTime}", DateTime.Now);
            return new PagedResponse<List<GetUserRoleDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new PagedResponse<List<GetUserRoleDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetUserRoleDto>> GetUserRoleByIdAsync(int id)
    {
        try
        {

            logger.LogInformation("GetUserRoleById method started at {DateTime}", DateTime.Now);
            var existing = await context.UserRoles.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) { 
                logger.LogWarning("UserRole not found {Id} at {DateTime}", id, DateTime.Now);
                return new Response<GetUserRoleDto>(HttpStatusCode.BadRequest, "UserRole not found"); }
            var UserRoles = mapper.Map<GetUserRoleDto>(existing);
            logger.LogInformation("GetUserRoleById method finished at {DateTime}", DateTime.Now);
            return new Response<GetUserRoleDto>(UserRoles);
        }
        catch (Exception e)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<GetUserRoleDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateUserRoleAsync(UpdateUserRoleDto updateUserRoleDto)
    {
        try
        {
            logger.LogInformation("UpdateUserRole method started at {DateTime}", DateTime.Now);
            var existing = await context.UserRoles.AnyAsync(e => e.Id == updateUserRoleDto.Id);
            if (!existing) return new Response<string>(HttpStatusCode.BadRequest, "UserRole not found!");
            var mapped = mapper.Map<UserRole>(updateUserRoleDto);
            context.UserRoles.Update(mapped);
            await context.SaveChangesAsync();
            logger.LogInformation("UpdateUserRole method finished at {DateTime}", DateTime.Now);
            return new Response<string>("Updated successfully");
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
