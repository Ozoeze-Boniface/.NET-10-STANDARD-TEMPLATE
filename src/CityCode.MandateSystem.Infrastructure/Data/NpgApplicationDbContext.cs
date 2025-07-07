using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Npgsql;
using CityCode.MandateSystem.Application.Common.Interfaces;

namespace CityCode.MandateSystem.Infrastructure.Data;

public class NpgApplicationDbContext<T> : INpgApplicationDbContext<T> where T : class, new()
{
    private readonly string _connectionString;

    public NpgApplicationDbContext(IConfiguration configuration)
    {
        this._connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<List<T>> GetAllAccountLedger(string query)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new NpgsqlCommand(query, connection))
            {
                return await ExecuteCommandAsync(command);
            }
        }    
    }

    private async Task<List<T>> ExecuteCommandAsync(NpgsqlCommand command)
    {
        using (var reader = await command.ExecuteReaderAsync())
                {
                    var results = new List<T>();
                    var dtoType = typeof(T);
                    var properties = dtoType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    while (await reader.ReadAsync())
                    {
                       var dto = Activator.CreateInstance<T>();

                        foreach (var property in properties)
                        {
                            try{
                            var ordinal = reader.GetOrdinal(property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name);
                            if (ordinal >= 0 && !reader.IsDBNull(ordinal))
                            {
                                var value = reader.GetValue(ordinal);
                                var convertedValue = Convert.ChangeType(value, property.PropertyType);
                                property.SetValue(dto, convertedValue);
                            }
                            }
                            catch(Exception ex){
                                Console.WriteLine(ex.Message);
                            }

                        }
                        results.Add(dto);
                    }

                    return results;
                }
    }
}