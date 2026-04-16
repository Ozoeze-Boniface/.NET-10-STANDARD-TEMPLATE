namespace KeyRails.BankingApi.Application.FunctionalTests;
using System.Data.Common;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Respawn;

using KeyRails.BankingApi.Infrastructure.Data;

public class SqlServerTestDatabase : ITestDatabase
{
    private readonly string _connectionString;
    private SqlConnection _connection = null!;
    private Respawner _respawner = null!;

    public SqlServerTestDatabase()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString);

        this._connectionString = connectionString;
    }

    public async Task InitialiseAsync()
    {
        this._connection = new SqlConnection(this._connectionString);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(this._connectionString)
            .Options;

        var context = new ApplicationDbContext(options);

        context.Database.Migrate();

        this._respawner = await Respawner.CreateAsync(this._connectionString, new RespawnerOptions
        {
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
    }

    public DbConnection GetConnection() => this._connection;

    public async Task ResetAsync() => await this._respawner.ResetAsync(this._connectionString);

    public async Task DisposeAsync() => await this._connection.DisposeAsync();
}
