namespace CityCode.MandateSystem.Web.Endpoints;
using CityCode.MandateSystem.Application.TodoItems.Commands.CreateTodoItem;
using CityCode.MandateSystem.Application.TodoItems.Commands.DeleteTodoItem;
using CityCode.MandateSystem.Application.TodoItems.Commands.UpdateTodoItem;
using CityCode.MandateSystem.Application.TodoItems.Commands.UpdateTodoItemDetail;
using CityCode.MandateSystem.Application.TodoItems.Queries.GetTodoItemsWithPagination;


public static class TodoItemEndpoints
{
    public static RouteGroupBuilder TodoItemGroup(this RouteGroupBuilder group)
    {
        group.MapPost("/CreateTodoItem", async (ISender sender, CreateTodoItemCommand command) => await sender.Send(command)).WithName("CreateUserItems")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Create Todo Items for users.",
            Description = "This allows creation of todo items"
        });

        group.MapPut("/UpdateTodoItem", async (ISender sender, int id, UpdateTodoItemCommand command) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest();
                }

                await sender.Send(command);
                return Results.NoContent();
            });

        group.MapPut("/UpdateTodoItemDetail", async (ISender sender, int id, UpdateTodoItemDetailCommand command) =>
                {
                    if (id != command.Id)
                    {
                        return Results.BadRequest();
                    }

                    await sender.Send(command);
                    return Results.NoContent();
                });

        group.MapDelete("/DeleteTodoItem", async (ISender sender, int id) =>
                       {
                           await sender.Send(new DeleteTodoItemCommand(id));
                           return Results.NoContent();
                       });

        group.MapGet("/GetTodoItemsWithPagination", async (ISender sender, [AsParameters] GetTodoItemsWithPaginationQuery query) => await sender.Send(query));

        return group;
    }

}
