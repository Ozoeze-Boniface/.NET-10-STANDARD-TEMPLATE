namespace KeyRails.BankingApi.Application.FunctionalTests;
using System.Data.Common;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Respawn;

using KeyRails.BankingApi.Infrastructure.Data;

using Testcontainers.MsSql;

public class TestcontainersTestDatabase : ITestDatabase
{
    private readonly MsSqlContainer _container;
    private DbConnection _connection = null!;
    private string _connectionString = null!;
    private Respawner _respawner = null!;

    public TestcontainersTestDatabase() => this._container = new MsSqlBuilder()
            .WithAutoRemove(true)
            .Build();

    public async Task InitialiseAsync()
    {
        await this._container.StartAsync();

        this._connectionString = this._container.GetConnectionString();

        this._connection = new SqlConnection(this._connectionString);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(this._connectionString)
            .Options;

        using var context = new ApplicationDbContext(options);

        context.Database.Migrate();

        this._respawner = await Respawner.CreateAsync(this._connectionString, new RespawnerOptions
        {
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
    }

    public DbConnection GetConnection() => this._connection;

    public async Task ResetAsync() => await this._respawner.ResetAsync(this._connectionString);

    public async Task DisposeAsync()
    {
        await this._connection.DisposeAsync();
        await this._container.DisposeAsync();
    }
}
