using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Api.Extentions;
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
            .WithDisplayName("Create User").RequireAuthorization().RequirePermission(PermissionConstants.CreateUser);

            return group;
        }
    }
}