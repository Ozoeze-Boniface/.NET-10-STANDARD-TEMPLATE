using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KeyRails.BankingApi.Application.Query;

namespace KeyRails.BankingApi.Application.Extentions
{
    public static class QueryExtentions
    {

        public static IQueryable<Activity> ApplyActivityFilter(this IQueryable<Activity> query,
            GetActivityLogQuery request)
        {
            if (request.ActorId.HasValue)
            {
                query = query.Where(x => x.Actor == request.ActorId);
            }

            if (request.StartDate.HasValue)
            {
                query = query.Where(x => x.DateCreated >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(x => x.DateCreated <= request.EndDate.Value);
            }

            return query.OrderByDescending(x => x.DateCreated);
        }

        public static IQueryable<User> ApplyUserFilter(
           this IQueryable<User> query,
           GetUsersQuery request)
        {
            if (request == null)
                return query;

            if (request.UserId is not null)
                query = query.Where(x => x.UserId == request.UserId);

            if (request.Active is not null)
                query = query.Where(x => x.IsActive == request.Active);

            if (request.StartDate is not null)
                query = query.Where(x => x.DateCreated >= request.StartDate);

            if (request.EndDate is not null)
                query = query.Where(x => x.DateCreated <= request.EndDate);

            return query.OrderByDescending(p => p.DateCreated);
        }
    }
}