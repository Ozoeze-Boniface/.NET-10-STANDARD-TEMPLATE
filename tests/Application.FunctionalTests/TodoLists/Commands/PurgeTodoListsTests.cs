namespace KeyRails.BankingApi.Application.FunctionalTests.TodoLists.Commands;
using KeyRails.BankingApi.Application.Common.Exceptions;
using KeyRails.BankingApi.Application.Common.Security;
using KeyRails.BankingApi.Application.TodoLists.Commands.CreateTodoList;
using KeyRails.BankingApi.Application.TodoLists.Commands.PurgeTodoLists;
using KeyRails.BankingApi.Domain.Entities;
using static Testing;

public class PurgeTodoListsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
       
    }

    [Test]
    public async Task ShouldDenyNonAdministrator()
    {

       
    }

    [Test]
    public async Task ShouldAllowAdministrator()
    {
       
    }

    [Test]
    public async Task ShouldDeleteAllLists()
    {
      
    }
}
