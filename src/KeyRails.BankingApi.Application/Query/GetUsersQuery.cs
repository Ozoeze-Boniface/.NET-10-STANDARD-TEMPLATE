using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyRails.BankingApi.Application.Common.Models;
using KeyRails.BankingApi.Application.Common.Models.View;

namespace KeyRails.BankingApi.Application.Query
{
    public class GetUsersQuery : IRequest<Common.Models.View.Result<PaginatedList<User>>>
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public long? UserId { get; set; }
        public bool? Active { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}