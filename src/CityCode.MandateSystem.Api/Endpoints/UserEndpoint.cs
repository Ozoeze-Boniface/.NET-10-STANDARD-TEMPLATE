using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Api.Extentions;
using CityCode.MandateSystem.Application.Query;
using CityCode.MandateSystem.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace CityCode.MandateSystem.Api.Endpoints
{
    public static class UserEndpoint
    {
        public static RouteGroupBuilder UserGroup(this RouteGroupBuilder group)
        {
            group.MapPost("/create-user", async (CreateUserCommand command, ISender sender) => // TODO; Add permission filter to allow only super admin to create user
            {
                var result = await sender.Send(command);
                return result;
            })
            .WithDisplayName("Create User").RequirePermission(PermissionConstants.CreateUser);

            group.MapGet("/get-users", async ([AsParameters]GetUsersQuery query, ISender sender) =>
           {
               var result = await sender.Send(query);
               return result;
           })
           .WithDisplayName("Get Users");

            group.MapPut("/activate-deactivate-user", async ([AsParameters]ActivateDeactivateUserCommand command, ISender sender) =>
           {
               var result = await sender.Send(command);
               return result;
           })
           .WithDisplayName("Activate or Deactivate Users").RequirePermission(PermissionConstants.ActivateDeactivateUser);

            group.MapDelete("/delete-user", async ([AsParameters] DeleteUserCommand command, ISender sender) =>
           {
               var result = await sender.Send(command);
               return result;
           })
           .WithDisplayName("delete Users").RequirePermission(PermissionConstants.EditUser);

            group.MapPut("/edit-user", async ([FromQuery] long userId, [FromBody] EditUserCommand command, ISender sender) =>
           {
                command.Id = userId;
               var result = await sender.Send(command);
               return result;
           })
           .WithDisplayName("edit Users").RequirePermission(PermissionConstants.EditUser);

            return group;
        }
    }
}