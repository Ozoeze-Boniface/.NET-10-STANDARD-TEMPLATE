using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;
using CityCode.MandateSystem.Application.Common.Interfaces;

namespace CityCode.MandateSystem.Infrastructure.Data
{
    public class DapperContext : IDapperContext
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public DapperContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(connectionString);
    }
}