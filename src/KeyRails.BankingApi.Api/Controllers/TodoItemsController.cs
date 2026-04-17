namespace KeyRails.BankingApi.Api.Controllers;

using KeyRails.BankingApi.Application.TodoItems.Commands.CreateTodoItem;
using KeyRails.BankingApi.Application.TodoItems.Commands.DeleteTodoItem;
using KeyRails.BankingApi.Application.TodoItems.Commands.UpdateTodoItem;
using KeyRails.BankingApi.Application.TodoItems.Commands.UpdateTodoItemDetail;
using KeyRails.BankingApi.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("todo")]
public class TodoItemsController : ApiControllerBase
{
    [HttpPost("CreateTodoItem")]
    public async Task<IActionResult> CreateTodoItem([FromBody] CreateTodoItemCommand command)
    {
        var todoItemId = await Sender.Send(command);
        return Ok(todoItemId);
    }

    [HttpPut("UpdateTodoItem")]
    public async Task<IActionResult> UpdateTodoItem([FromQuery] int id, [FromBody] UpdateTodoItemCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Sender.Send(command);
        return NoContent();
    }

    [HttpPut("UpdateTodoItemDetail")]
    public async Task<IActionResult> UpdateTodoItemDetail([FromQuery] int id, [FromBody] UpdateTodoItemDetailCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Sender.Send(command);
        return NoContent();
    }

    [HttpDelete("DeleteTodoItem")]
    public async Task<IActionResult> DeleteTodoItem([FromQuery] int id)
    {
        await Sender.Send(new DeleteTodoItemCommand(id));
        return NoContent();
    }

    [HttpGet("GetTodoItemsWithPagination")]
    public async Task<IActionResult> GetTodoItemsWithPagination([FromQuery] GetTodoItemsWithPaginationQuery query)
    {
        var todoItems = await Sender.Send(query);
        return Ok(todoItems);
    }
}
