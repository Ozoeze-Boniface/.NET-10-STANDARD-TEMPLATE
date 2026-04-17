namespace KeyRails.BankingApi.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;

public abstract class ApiControllerBase : ControllerBase
{
    private ISender? sender;

    protected ISender Sender => this.sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
