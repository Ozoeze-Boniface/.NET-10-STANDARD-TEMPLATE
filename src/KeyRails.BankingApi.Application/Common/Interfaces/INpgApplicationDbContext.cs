using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KeyRails.BankingApi.Domain.Enums;

namespace KeyRails.BankingApi.Application.Common.Interfaces
{
    public interface INpgApplicationDbContext<T> where T : class, new() 
    {
        Task<List<T>> GetAllAccountLedger(string query);

    }    
}