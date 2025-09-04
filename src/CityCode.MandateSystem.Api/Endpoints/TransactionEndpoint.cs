using CityCode.MandateSystem.Application.Query;

namespace CityCode.MandateSystem.Api.Endpoints
{
    public static class TransactionEndpoint
    {
        public static  RouteGroupBuilder TransactionGroup(this RouteGroupBuilder group)
        {
            group.MapGet("/get-transactions", async ([AsParameters] GetTransactionsQuery query, ISender sender) => {
                var result = await sender.Send(query);
                return result;
            }) .WithDisplayName("Get all transactions");

            return group;
        }
    }
}