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

        public static MandateTransactionPayload BuildMandateTransactionPayload(
            this Mandate mandate, string bankcode, decimal? amount = null)
        {
            var transactionAmount = amount is null
                ? mandate.TransactionAmount.ToString("F2")
                : amount.Value.ToString("F2");

            return new MandateTransactionPayload
            {
                Amount = transactionAmount,
                BeneficiaryAccountName = mandate.BanksAccountName,
                BeneficiaryAccountNumber = mandate.BanksAccountNumber,
                BeneficiaryBankVerificationNumber = mandate.BanksBvn,
                BeneficiaryKYCLevel = "1",
                ChannelCode = "1",
                OriginatorAccountName = mandate.PayerName,
                OriginatorAccountNumber = mandate.PayerAccountNumber,
                OriginatorKYCLevel = "1",
                MandateReferenceNumber = mandate.NibbsMandateCode,
                PaymentReference =
                    $"{mandate.NibbsMandateCode}/{mandate.BillerId}/{mandate.ProductId}/{mandate.MandateId}",
                TransactionLocation = mandate.Location,
                OriginatorNarration = mandate.Narration,
                BeneficiaryNarration = mandate.Narration,
                BillerId = mandate.BillerId.ToString(),
                DestinationInstitutionCode = mandate.DestinationInstitutionCode,
                SourceInstitutionCode = mandate.SourceInstitutionCode,
                TransactionId = Helpers.GenerateTransactionId(bankcode),
                OriginatorBankVerificationNumber = mandate.PayerBvn,
                NameEnquiryRef = Helpers.GenerateTransactionId(bankcode),
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

        public class MandateTransactionPayload
        {
            public string? Amount { get; set; }
            public string? BeneficiaryAccountName { get; set; }
            public string? BeneficiaryAccountNumber { get; set; }
            public string? BeneficiaryBankVerificationNumber { get; set; }
            public string? BeneficiaryKYCLevel { get; set; }
            public string? ChannelCode { get; set; }
            public string? OriginatorAccountName { get; set; }
            public string? OriginatorAccountNumber { get; set; }
            public string? OriginatorKYCLevel { get; set; }
            public string? MandateReferenceNumber { get; set; }
            public string? PaymentReference { get; set; }
            public string? TransactionLocation { get; set; }
            public string? OriginatorNarration { get; set; }
            public string? BeneficiaryNarration { get; set; }
            public string? BillerId { get; set; }
            public string? DestinationInstitutionCode { get; set; }
            public string? SourceInstitutionCode { get; set; }
            public string? TransactionId { get; set; }
            public string? OriginatorBankVerificationNumber { get; set; }
            public string? NameEnquiryRef { get; set; }
            public string? InitiatorAccountName { get; set; }
            public string? InitiatorAccountNumber { get; set; }
        }
    }
}