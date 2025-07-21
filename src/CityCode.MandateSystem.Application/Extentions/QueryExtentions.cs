using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Query;

namespace CityCode.MandateSystem.Application.Extentions
{
    public static class QueryExtentions
    {
        public static IQueryable<MandateRequest> ApplyFilters(
            this IQueryable<MandateRequest> query,
            GetMandateRequestQuery request)
        {
            if (request.MandateRequestId is not null)
                query = query.Where(x => x.MandateRequestId == request.MandateRequestId);

            if (!string.IsNullOrWhiteSpace(request.MandateReference))
                query = query.Where(x => x.MandateReference == request.MandateReference);

            if (request.ProductId is not null)
                query = query.Where(x => x.ProductId == request.ProductId);

            if (request.BillerId is not null)
                query = query.Where(x => x.BillerId == request.BillerId);

            if (!string.IsNullOrWhiteSpace(request.SubscriberCode))
                query = query.Where(x => x.SubscriberCode == request.SubscriberCode);

            if (request.ProductTotalAmount is not null)
                query = query.Where(x => x.ProductTotalAmount == request.ProductTotalAmount);

            if (request.TransactionAmount is not null)
                query = query.Where(x => x.TransactionAmount == request.TransactionAmount);

            if (!string.IsNullOrWhiteSpace(request.BankCode))
                query = query.Where(x => x.BankCode == request.BankCode);

            if (!string.IsNullOrWhiteSpace(request.PayerName))
                query = query.Where(x => x.PayerName.Contains(request.PayerName));

            if (!string.IsNullOrWhiteSpace(request.PayerAddress))
                query = query.Where(x => x.PayerAddress.Contains(request.PayerAddress));

            if (!string.IsNullOrWhiteSpace(request.PayerEmail))
                query = query.Where(x => x.PayerEmail == request.PayerEmail);

            if (!string.IsNullOrWhiteSpace(request.PayerPhoneNumber))
                query = query.Where(x => x.PayerPhoneNumber == request.PayerPhoneNumber);

            if (!string.IsNullOrWhiteSpace(request.AccountName))
                query = query.Where(x => x.AccountName == request.AccountName);

            if (!string.IsNullOrWhiteSpace(request.PayerAccountNumber))
                query = query.Where(x => x.PayerAccountNumber == request.PayerAccountNumber);

            if (!string.IsNullOrWhiteSpace(request.PayerBvn))
                query = query.Where(x => x.PayerBvn == request.PayerBvn);

            if (!string.IsNullOrWhiteSpace(request.BanksAccountNumber))
                query = query.Where(x => x.BanksAccountNumber == request.BanksAccountNumber);

            if (!string.IsNullOrWhiteSpace(request.BanksAccountName))
                query = query.Where(x => x.BanksAccountName == request.BanksAccountName);

            if (!string.IsNullOrWhiteSpace(request.BanksBvn))
                query = query.Where(x => x.BanksBvn == request.BanksBvn);

            if (!string.IsNullOrWhiteSpace(request.DestinationInstitutionCode))
                query = query.Where(x => x.DestinationInstitutionCode == request.DestinationInstitutionCode);

            if (!string.IsNullOrWhiteSpace(request.SourceInstitutionCode))
                query = query.Where(x => x.SourceInstitutionCode == request.SourceInstitutionCode);

            if (!string.IsNullOrWhiteSpace(request.Narration))
                query = query.Where(x => x.Narration.Contains(request.Narration));

            if (request.MandateType is not null)
                query = query.Where(x => x.MandateType == request.MandateType);

            if (request.MandateRequestStatus is not null)
                query = query.Where(x => x.MandateRequestStatus == request.MandateRequestStatus);

            if (request.StartDate is not null)
                query = query.Where(x => x.StartDate >= request.StartDate);

            if (request.EndDate is not null)
                query = query.Where(x => x.EndDate <= request.EndDate);

            if (request.PaymentFrequency is not null)
                query = query.Where(x => x.PaymentFrequency == request.PaymentFrequency);

            if (!string.IsNullOrWhiteSpace(request.Location))
                query = query.Where(x => x.Location.Contains(request.Location));

            query = query.ApplySearch(request.SearchField, request.SearchTerm);

            return query;
        }

        public static IQueryable<Mandate> ApplyFilters(
            this IQueryable<Mandate> query,
            GetMandateQuery request)
        {
            if (request.MandateId is not null)
                query = query.Where(x => x.MandateId == request.MandateId);

            if (!string.IsNullOrWhiteSpace(request.MandateReference))
                query = query.Where(x => x.MandateReference == request.MandateReference);

            if (!string.IsNullOrWhiteSpace(request.NibbsMandateCode))
                query = query.Where(x => x.NibbsMandateCode == request.NibbsMandateCode);

            if (request.ProductId is not null)
                query = query.Where(x => x.ProductId == request.ProductId);

            if (request.BillerId is not null)
                query = query.Where(x => x.BillerId == request.BillerId);

            if (request.WorkflowStatus is not null)
                query = query.Where(x => x.WorkflowStatus == request.WorkflowStatus);

            if (request.MandateStatus is not null)
                query = query.Where(x => x.MandateStatus == request.MandateStatus);

            if (request.ProgressStatus is not null)
                query = query.Where(x => x.ProgressStatus == request.ProgressStatus);

            if (!string.IsNullOrWhiteSpace(request.SubscriberCode))
                query = query.Where(x => x.SubscriberCode == request.SubscriberCode);

            if (request.ProductTotalAmount is not null)
                query = query.Where(x => x.ProductTotalAmount == request.ProductTotalAmount);

            if (request.TransactionAmount is not null)
                query = query.Where(x => x.TransactionAmount == request.TransactionAmount);

            if (!string.IsNullOrWhiteSpace(request.BankCode))
                query = query.Where(x => x.BankCode == request.BankCode);

            if (!string.IsNullOrWhiteSpace(request.PayerName))
                query = query.Where(x => x.PayerName.Contains(request.PayerName));

            if (!string.IsNullOrWhiteSpace(request.PayerAddress))
                query = query.Where(x => x.PayerAddress.Contains(request.PayerAddress));

            if (!string.IsNullOrWhiteSpace(request.PayerEmail))
                query = query.Where(x => x.PayerEmail == request.PayerEmail);

            if (!string.IsNullOrWhiteSpace(request.PayerPhoneNumber))
                query = query.Where(x => x.PayerPhoneNumber == request.PayerPhoneNumber);

            if (!string.IsNullOrWhiteSpace(request.AccountName))
                query = query.Where(x => x.AccountName == request.AccountName);

            if (!string.IsNullOrWhiteSpace(request.PayerAccountNumber))
                query = query.Where(x => x.PayerAccountNumber == request.PayerAccountNumber);

            if (!string.IsNullOrWhiteSpace(request.PayerBvn))
                query = query.Where(x => x.PayerBvn == request.PayerBvn);

            if (!string.IsNullOrWhiteSpace(request.BanksAccountNumber))
                query = query.Where(x => x.BanksAccountNumber == request.BanksAccountNumber);

            if (!string.IsNullOrWhiteSpace(request.BanksAccountName))
                query = query.Where(x => x.BanksAccountName == request.BanksAccountName);

            if (!string.IsNullOrWhiteSpace(request.BanksBvn))
                query = query.Where(x => x.BanksBvn == request.BanksBvn);

            if (!string.IsNullOrWhiteSpace(request.DestinationInstitutionCode))
                query = query.Where(x => x.DestinationInstitutionCode == request.DestinationInstitutionCode);

            if (!string.IsNullOrWhiteSpace(request.SourceInstitutionCode))
                query = query.Where(x => x.SourceInstitutionCode == request.SourceInstitutionCode);

            if (!string.IsNullOrWhiteSpace(request.Narration))
                query = query.Where(x => x.Narration.Contains(request.Narration));

            if (request.MandateType is not null)
                query = query.Where(x => x.MandateType == request.MandateType);

            if (request.StartDate is not null)
                query = query.Where(x => x.StartDate >= request.StartDate);

            if (request.EndDate is not null)
                query = query.Where(x => x.EndDate <= request.EndDate);

            if (request.PaymentFrequency is not null)
                query = query.Where(x => x.PaymentFrequency == request.PaymentFrequency);

            if (!string.IsNullOrWhiteSpace(request.Location))
                query = query.Where(x => x.Location.Contains(request.Location));

            query = query.ApplySearch(request.SearchField, request.SearchTerm);


            return query;
        }

        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            return query.Skip(skip).Take(pageSize);
        }

        public static IQueryable<T> ApplySearch<T>(
        this IQueryable<T> query,
        string? searchField,
        string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchField) || string.IsNullOrWhiteSpace(searchTerm))
                return query;

            var property = typeof(T).GetProperty(searchField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null || property.PropertyType != typeof(string))
                return query; // Skip invalid property or non-string fields

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, property.Name);
            var searchTermConstant = Expression.Constant(searchTerm, typeof(string));

            var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
            var containsCall = Expression.Call(propertyAccess, containsMethod, searchTermConstant);

            var lambda = Expression.Lambda<Func<T, bool>>(containsCall, parameter);
            return query.Where(lambda);
        }

    }
}