using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Query;

namespace CityCode.MandateSystem.Api.Endpoints
{
    public static class ActivityEndpoint
    {
        public static RouteGroupBuilder ActivityGroup(this RouteGroupBuilder group)
        {
            group.MapGet("/get-activity-logs", async ([AsParameters]GetActivityLogQuery query, ISender sender) => // TODO; Add permission filter to allow only super admin to create user
            {
                var result = await sender.Send(query);
                return result;
            })
            .WithDisplayName("Get Activity Log");

            return group;
        }
    }
}