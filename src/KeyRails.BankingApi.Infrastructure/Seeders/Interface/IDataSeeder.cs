using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyRails.BankingApi.Infrastructure.Seeders
{
    public interface IDataSeeder
    {
        Task SeedAsync();
        int Order { get; }
    }
}