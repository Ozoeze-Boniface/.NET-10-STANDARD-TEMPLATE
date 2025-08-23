using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CityCode.MandateSystem.Api.Extentions
{
    public static class PermissionFilter
    {
        public static RouteHandlerBuilder RequirePermission(this RouteHandlerBuilder builder, string permission)
        {
            return builder.RequirePermissions(PermissionLogic.And, permission);
        }

        // Extension method for multiple permissions
        public static RouteHandlerBuilder RequirePermissions(this RouteHandlerBuilder builder, PermissionLogic logic, params string[] permissions)
        {
            return builder.AddEndpointFilter(async (context, next) =>
            {
                var httpContext = context.HttpContext;
                
                var httpRequest = context.HttpContext.Request;
                var currentUser = httpRequest.GetCurrentUser();

                if (!currentUser.IsAuthenticated)
                {
                    return Results.Unauthorized();
                }

                // Super users have access to everything
                if (currentUser.User.IsSuperAdmin)
                {
                    return await next(context);
                }

                var userPermissions = currentUser.Permissions?.Select(p => p.Name).ToList() ?? new List<string>();

                bool hasRequiredPermissions = logic switch
                {
                    PermissionLogic.And => permissions.All(p => userPermissions.Contains(p)),
                    PermissionLogic.Or => permissions.Any(p => userPermissions.Contains(p)),
                    _ => false
                };

                if (!hasRequiredPermissions)
                {
                    return Results.Forbid();
                }

                return await next(context);
            });
        }

        // Convenience extension methods for common scenarios
        public static RouteHandlerBuilder RequireAnyPermission(this RouteHandlerBuilder builder, params string[] permissions)
        {
            return builder.RequirePermissions(PermissionLogic.Or, permissions);
        }

        public static RouteHandlerBuilder RequireAllPermissions(this RouteHandlerBuilder builder, params string[] permissions)
        {
            return builder.RequirePermissions(PermissionLogic.And, permissions);
        }
    }
}