namespace CityCode.MandateSystem.Application.TodoLists.Queries.GetTodos;

using CityCode.MandateSystem.Application.Common.Models;

public class TodosVm
{
    public IReadOnlyCollection<LookupDto> PriorityLevels { get; init; } = [];

    public IReadOnlyCollection<TodoListDto> Lists { get; init; } = [];
}
