using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CityCode.MandateSystem.Domain.Enums;

namespace CityCode.MandateSystem.Application.Common.Interfaces
{
    public interface INpgApplicationDbContext<T> where T : class, new() 
    {
        Task<List<T>> GetAllAccountLedger(string query);

    }    
}