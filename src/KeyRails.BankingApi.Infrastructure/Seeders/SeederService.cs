using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyRails.BankingApi.Infrastructure.Seeders.Interface;
using Microsoft.EntityFrameworkCore;

namespace KeyRails.BankingApi.Infrastructure.Seeders
{
    public class SeederService : ISeederService
    {
        private readonly IEnumerable<IDataSeeder> _seeders;

        public SeederService(
            IEnumerable<IDataSeeder> seeders)
        {
            _seeders = seeders;
        }

        public async Task SeedAllAsync()
        {

            var seeders = _seeders.OrderBy(s => s.Order);
            foreach (var seeder in seeders)
            {
                try
                {
                    await seeder.SeedAsync();
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex);
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex);
                }

            }
        }
    }
}