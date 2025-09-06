using CityCode.MandateSystem.Api.Extentions;
using CityCode.MandateSystem.Application.Query;
using CityCode.MandateSystem.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace CityCode.MandateSystem.Api.Endpoints
{
    public static class MandateEndpoint
    {
        public static RouteGroupBuilder MandateGroup(this RouteGroupBuilder group)
        {
            group.MapPost("/initiate-mandate", async (CreateMandateCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result;
            })
            .WithDisplayName("Create mandate").RequirePermission(PermissionConstants.CreateMandate);

            group.MapPost("/approve-mandate", async (ApproveMandateCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result;
            })
            .WithDisplayName("Approve mandate").RequirePermission(PermissionConstants.ApproveMandate);

            group.MapGet("/get-mandate-request", async ([AsParameters] GetMandateRequestQuery command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result;
            })
            .WithDisplayName("Get Requests");

            group.MapGet("/get-mandates", async ([AsParameters] GetMandateQuery command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result;
            })
            .WithDisplayName("Get Mandates");

            group.MapGet("/get-banks", async (ISender sender) =>
            {
                var query = new GetBankQuery();
                var result = await sender.Send(query);
                return result;
            })
            .WithDisplayName("Get Banks");

            group.MapPost("/do-name-enquiry", async (ISender sender, [FromBody] DoNameEnquiryCommand command) =>
            {
                var result = await sender.Send(command);
                return result;
            })
            .WithDisplayName("Do name enquiry");

             group.MapPost("/deactivate-mandate", async (ISender sender, [FromBody] DeactivateMandateCommand command) =>
            {
                var result = await sender.Send(command);
                return result;
            })
            .WithDisplayName("Deactivate mandate");

            return group;
        }

    }
}