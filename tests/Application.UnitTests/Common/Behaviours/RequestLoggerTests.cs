namespace KeyRails.BankingApi.Application.UnitTests.Common.Behaviours;
using Microsoft.Extensions.Logging;

using Moq;
using NUnit.Framework;

using KeyRails.BankingApi.Application.Common.Behaviours;
using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Application.TodoItems.Commands.CreateTodoItem;


public class RequestLoggerTests
{
    private Mock<ILogger<CreateTodoItemCommand>> _logger = null!;
    private Mock<IUser> _user = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    {
        this._logger = new Mock<ILogger<CreateTodoItemCommand>>();
        this._user = new Mock<IUser>();
        this._identityService = new Mock<IIdentityService>();
    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        this._user.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());

        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand>(this._logger.Object, this._user.Object, this._identityService.Object);

        await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());

        this._identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand>(this._logger.Object, this._user.Object, this._identityService.Object);

        await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());

        this._identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
    }
}
