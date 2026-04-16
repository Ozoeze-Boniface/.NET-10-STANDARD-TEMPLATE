namespace KeyRails.BankingApi.Application.FunctionalTests.TodoLists.Commands;
using KeyRails.BankingApi.Application.TodoLists.Commands.CreateTodoList;
using KeyRails.BankingApi.Application.TodoLists.Commands.DeleteTodoList;
using KeyRails.BankingApi.Domain.Entities;
using static Testing;

public class DeleteTodoListTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new DeleteTodoListCommand(99);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTodoList()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        await SendAsync(new DeleteTodoListCommand(listId));

        var list = await FindAsync<TodoList>(listId);

        list.Should().BeNull();
    }
}
