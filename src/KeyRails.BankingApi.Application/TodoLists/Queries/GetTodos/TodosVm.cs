namespace KeyRails.BankingApi.Application.TodoLists.Queries.GetTodos;

using KeyRails.BankingApi.Application.Common.Models;

public class TodosVm
{
    public IReadOnlyCollection<LookupDto> PriorityLevels { get; init; } = [];

    public IReadOnlyCollection<TodoListDto> Lists { get; init; } = [];
}
