using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Settings
{
    public class SystemSettings
    {
        public int ProductId { get; set; }
        public int BillerId { get; set; }
        public string BankCode { get; set; } = string.Empty;
        public string FeeAccountNumber { get; set; } = string.Empty;
        public string FeeAccountName { get; set; } = string.Empty;
        public string FeeAccountBVN { get; set; } = string.Empty;
        public EmailSettings? EmailSettings { get; set; }
        public NibssSettings? NibssSettings { get; set; }
    }

    public class NibssSettings
    {
        public string CreateMandateUrl { get; set; } = string.Empty;
        public string UpdateMandateStatusUrl { get; set; } = string.Empty;
        public string MandateStatusUrl { get; set; } = string.Empty;
        // Auth-related values
        public string TokenUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Cookie { get; set; } = string.Empty;
        public FundsTransferSettings? FundsTransferSettings { get; set; }
    }
    public class FundsTransferSettings
    {
        public string TransferBaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Cookie { get; set; } = string.Empty;
    }
}