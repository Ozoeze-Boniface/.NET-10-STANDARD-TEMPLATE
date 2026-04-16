namespace KeyRails.BankingApi.Web.Endpoints;

public static class ApiRequestLogEndpoints
{
    public static RouteGroupBuilder ApiRequestLogGroup(this RouteGroupBuilder group)
    {
        group.MapPost("/CreateApiRequestLog", async (ISender sender, CreateApiRequestLogCommand command) => await sender.Send(command)).WithName("CreateApiRequestLog")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Create ApiRequestLog for.",
            Description = "This allows creation of ApiRequestLog items"
        });

        group.MapPut("/UpdateApiRequestLog", async (ISender sender, int id, UpdateApiRequestLogCommand command) =>
            {
                if (id != command.ApiRequestLogId)
                {
                    return Results.BadRequest();
                }

                await sender.Send(command);
                return Results.NoContent();
            });

        group.MapDelete("/DeleteApiRequestLog", async (ISender sender, int id) =>
                       {
                           await sender.Send(new DeleteApiRequestLogCommand(id));
                           return Results.NoContent();
                       });

        group.MapGet("/GetApiRequestLogsWithPagination", async (ISender sender, [AsParameters] GetApiRequestLogsWithPaginationQuery query) => await sender.Send(query));

        return group;
    }

}
