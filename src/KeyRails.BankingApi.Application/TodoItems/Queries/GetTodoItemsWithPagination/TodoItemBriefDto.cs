namespace KeyRails.BankingApi.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using KeyRails.BankingApi.Domain.Entities;

public class TodoItemBriefDto
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }

    private sealed class Mapping : Profile
    {
        public Mapping() => this.CreateMap<TodoItem, TodoItemBriefDto>();
    }
}
