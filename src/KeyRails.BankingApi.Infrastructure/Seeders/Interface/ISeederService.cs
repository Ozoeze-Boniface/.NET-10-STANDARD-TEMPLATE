using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyRails.BankingApi.Infrastructure.Seeders.Interface
{
    public interface ISeederService
    {
        Task SeedAllAsync();
    }
}