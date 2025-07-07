namespace CityCode.MandateSystem.Application.UnitTests.Common.Mappings;
using System.Reflection;
using System.Runtime.Serialization;
using AutoMapper;
using NUnit.Framework;
using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Application.Common.Models;
using CityCode.MandateSystem.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using CityCode.MandateSystem.Application.TodoLists.Queries.GetTodos;
using CityCode.MandateSystem.Domain.Entities;

public class MappingTests
{
    private readonly IConfigurationProvider configuration;
    private readonly IMapper mapper;

    public MappingTests()
    {
        this.configuration = new MapperConfiguration(config =>
            config.AddMaps(Assembly.GetAssembly(typeof(IApplicationDbContext))));

        this.mapper = this.configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration() => this.configuration.AssertConfigurationIsValid();

    [Test]
    [TestCase(typeof(TodoList), typeof(TodoListDto))]
    [TestCase(typeof(TodoItem), typeof(TodoItemDto))]
    [TestCase(typeof(TodoList), typeof(LookupDto))]
    [TestCase(typeof(TodoItem), typeof(LookupDto))]
    [TestCase(typeof(TodoItem), typeof(TodoItemBriefDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = this.GetInstanceOf(source);

        this.mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
        {
            return Activator.CreateInstance(type)!;
        }

        // Type without parameterless constructor
        // TODO: Figure out an alternative approach to the now obsolete `FormatterServices.GetUninitializedObject` method.
#pragma warning disable SYSLIB0050 // Type or member is obsolete
        return FormatterServices.GetUninitializedObject(type);
#pragma warning restore SYSLIB0050 // Type or member is obsolete
    }
}
