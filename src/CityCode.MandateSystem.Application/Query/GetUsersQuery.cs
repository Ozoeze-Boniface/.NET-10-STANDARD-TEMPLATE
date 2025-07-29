using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Models;
using CityCode.MandateSystem.Application.Common.Models.View;

namespace CityCode.MandateSystem.Application.Query
{
    public class GetUsersQuery : IRequest<Common.Models.View.Result<PaginatedList<User>>>
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public long? UserId { get; set; }
        public bool? Active { get; set; }
    }
}