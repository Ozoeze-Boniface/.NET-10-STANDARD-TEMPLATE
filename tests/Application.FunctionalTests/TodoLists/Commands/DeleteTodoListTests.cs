namespace CityCode.MandateSystem.Application.FunctionalTests.TodoLists.Commands;
using CityCode.MandateSystem.Application.TodoLists.Commands.CreateTodoList;
using CityCode.MandateSystem.Application.TodoLists.Commands.DeleteTodoList;
using CityCode.MandateSystem.Domain.Entities;
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
