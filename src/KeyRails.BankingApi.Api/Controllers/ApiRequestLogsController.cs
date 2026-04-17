namespace KeyRails.BankingApi.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api-request-log")]
public class ApiRequestLogsController : ApiControllerBase
{
    [HttpPost("CreateApiRequestLog")]
    public async Task<IActionResult> CreateApiRequestLog([FromBody] CreateApiRequestLogCommand command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpPut("UpdateApiRequestLog")]
    public async Task<IActionResult> UpdateApiRequestLog([FromQuery] int id, [FromBody] UpdateApiRequestLogCommand command)
    {
        if (id != command.ApiRequestLogId)
        {
            return BadRequest();
        }

        await Sender.Send(command);
        return NoContent();
    }

    [HttpDelete("DeleteApiRequestLog")]
    public async Task<IActionResult> DeleteApiRequestLog([FromQuery] int id)
    {
        await Sender.Send(new DeleteApiRequestLogCommand(id));
        return NoContent();
    }

    [HttpGet("GetApiRequestLogsWithPagination")]
    public async Task<IActionResult> GetApiRequestLogsWithPagination([FromQuery] GetApiRequestLogsWithPaginationQuery query)
    {
        var apiRequestLogs = await Sender.Send(query);
        return Ok(apiRequestLogs);
    }
}
