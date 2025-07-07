namespace CityCode.MandateSystem.Application.TodoLists.Queries.GetTodos;
using CityCode.MandateSystem.Domain.Entities;

public class TodoListDto
{
    public TodoListDto() => this.Items = [];

    public int Id { get; init; }

    public string? Title { get; init; }

    public string? Colour { get; init; }

    public IReadOnlyCollection<TodoItemDto> Items { get; init; }

    private sealed class Mapping : Profile
    {
        public Mapping() => this.CreateMap<TodoList, TodoListDto>();
    }
}
