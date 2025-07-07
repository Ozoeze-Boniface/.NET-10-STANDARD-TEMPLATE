using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Common.Interfaces
{
    public interface IDapperContext
    {
        System.Data.IDbConnection CreateConnection();
    }
}