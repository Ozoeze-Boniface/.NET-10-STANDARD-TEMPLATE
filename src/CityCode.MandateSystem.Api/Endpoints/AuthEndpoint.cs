using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CityCode.MandateSystem.Api.Endpoints
{
    public static class EocCasaEndpoint
    {
        public static RouteGroupBuilder AuthGroup(this RouteGroupBuilder group)
        {
            group.MapPost("/authenticate", async (AuthenticateUserCommand command, ISender send) =>
            {
                var result = await send.Send(command);
                return result;
            }).WithDisplayName("Authentication");

            group.MapPost("/initiate-reset-password", async (InitiateResetPasswordCommand command, ISender send) =>
            {
                var result = await send.Send(command);
                return result;
            }).WithDisplayName("Initiate Reset Password");

            group.MapPost("/reset-password", async (ResetPasswordCommand command, ISender send) =>
            {
                var result = await send.Send(command);
                return result;
            }).WithDisplayName("Reset Password");
            
            return group;
        }
    }
}