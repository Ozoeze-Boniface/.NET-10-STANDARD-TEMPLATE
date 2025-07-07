namespace CityCode.MandateSystem.Application.Common.Models;
using CityCode.MandateSystem.Domain.Entities;

public class LookupDto
{
    public int Id { get; init; }

    public string? Title { get; init; }

    private sealed class Mapping : Profile
    {
        public Mapping()
        {
            this.CreateMap<TodoList, LookupDto>();
            this.CreateMap<TodoItem, LookupDto>();
        }
    }
}
