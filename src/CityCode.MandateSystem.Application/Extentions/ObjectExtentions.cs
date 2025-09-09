using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Domain.Enums;

namespace CityCode.MandateSystem.Application.Extentions
{
    public static class ObjectExtentions
    {
        public static string CreateMandatePayload(this Mandate mandate)
        {
            var payload = new MandatePayloadDto
            {
                ProductId = mandate.ProductId,
                BillerId = mandate.BillerId,
                AccountNumber = mandate.PayerAccountNumber,
                BankCode = mandate.BankCode,
                PayerName = mandate.PayerName,
                MandateType = (int)mandate.MandateType,
                PayerAddress = mandate.PayerAddress,
                AccountName = mandate.AccountName,
                Amount = mandate.ProductTotalAmount,
                Frequency = mandate.PaymentFrequency.GetDaysFromFrequency(),
                Narration = mandate.Narration,
                PhoneNumber = mandate.PayerPhoneNumber,
                SubscriberCode = mandate.SubscriberCode,
                StartDate = mandate.StartDate.ToDateTime(TimeOnly.MinValue),
                EndDate = mandate.EndDate.ToDateTime(TimeOnly.MinValue),
                PayerEmail = mandate.PayerEmail
            };

            return JsonConvert.SerializeObject(payload);

        }

        public static object BuildMandateTransactionPayload(this Mandate mandate, string bankcode, decimal? amount = null)
        {
            var transactionAmount = amount is null ? mandate.TransactionAmount.ToString("F2") : amount.ToString();
            return new
            {
                amount = transactionAmount,
                beneficiaryAccountName = mandate.BanksAccountName,
                beneficiaryAccountNumber = mandate.BanksAccountNumber,
                beneficiaryBankVerificationNumber = mandate.BanksBvn,
                beneficiaryKYCLevel = "1", // you can map from another property if available
                channelCode = "1", // static or map if you store it
                originatorAccountName = mandate.PayerName,
                originatorAccountNumber = mandate.PayerAccountNumber,
                originatorKYCLevel = "1", // static or mapped
                mandateReferenceNumber = mandate.MandateReference,
                paymentReference = $"{mandate.MandateReference}/{mandate.BillerId}/{mandate.ProductId}/{mandate.MandateId}",
                transactionLocation = mandate.Location,
                originatorNarration = mandate.Narration,
                beneficiaryNarration = mandate.Narration,
                billerId = mandate.BillerId.ToString(),
                destinationInstitutionCode = mandate.DestinationInstitutionCode,
                sourceInstitutionCode = mandate.SourceInstitutionCode,
                transactionId = Helpers.GenerateTransactionId(bankcode),
                originatorBankVerificationNumber = mandate.PayerBvn,
                nameEnquiryRef = Helpers.GenerateTransactionId(bankcode),
                InitiatorAccountName = mandate.AccountName,
                InitiatorAccountNumber = mandate.PayerAccountNumber
            };


        }

        public static int GetDaysFromFrequency(this PaymentFrequency paymentFrequency)
        {
            return paymentFrequency switch
            {
                PaymentFrequency.Daily => 1,
                PaymentFrequency.Weekly => 7,
                PaymentFrequency.Monthly => 30,
                PaymentFrequency.Yearly => 365,
                _ => throw new ArgumentOutOfRangeException(nameof(paymentFrequency), "Invalid payment frequency")
            };
        }


        public static string FormaMandateActivationPayload(this Mandate mandate)
        {
            var payload = new
            {
                mandateCode = mandate.NibbsMandateCode,
                billerId = mandate.BillerId,
                productId = mandate.ProductId,
                accountNumber = mandate.PayerAccountNumber,
                mandateStatus = "1"
            };

            return JsonConvert.SerializeObject(payload);
        }
    }
}