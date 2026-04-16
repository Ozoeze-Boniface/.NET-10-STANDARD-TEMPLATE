namespace KeyRails.BankingApi.Application.UnitTests.Common.Mappings;
using System.Reflection;
using System.Runtime.Serialization;
using AutoMapper;
using NUnit.Framework;
using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Application.Common.Models;
using KeyRails.BankingApi.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using KeyRails.BankingApi.Application.TodoLists.Queries.GetTodos;
using KeyRails.BankingApi.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

public class MappingTests
{
    private readonly IConfigurationProvider configuration;
    private readonly IMapper mapper;

    public MappingTests()
    {
        this.configuration = new MapperConfiguration(
            config => config.AddMaps(Assembly.GetAssembly(typeof(IApplicationDbContext))),
            NullLoggerFactory.Instance);

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
