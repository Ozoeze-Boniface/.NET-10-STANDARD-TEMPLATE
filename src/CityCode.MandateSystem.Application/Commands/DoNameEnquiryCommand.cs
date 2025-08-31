using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common;
using CityCode.MandateSystem.Application.Common.Models.View;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Application.Settings;
using Microsoft.Extensions.Options;

namespace CityCode.MandateSystem.Application.Commands
{
    public class DoNameEnquiryCommand : IRequest<Common.Models.View.Result<NameEnquiryResponse>>
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string ChannelCode { get; set; } = string.Empty;
        public string DestinationInstitutionCode { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
    }

    public class DoNameEnquiryCommandHanler(IMandateService mandateService, IOptions<SystemSettings> options) : IRequestHandler<DoNameEnquiryCommand, Common.Models.View.Result<NameEnquiryResponse>>
    {
        private readonly IMandateService _mandateService = mandateService;
        private readonly string _bankCode = options.Value.BankCode;

        public async Task<Common.Models.View.Result<NameEnquiryResponse>> Handle(DoNameEnquiryCommand request, CancellationToken cancellationToken)
        {
            request.TransactionId = Helpers.GenerateTransactionId(_bankCode);
            var result = await _mandateService.DoNameEnquiry(request);

            return Common.Models.View.Result<NameEnquiryResponse>.Success(DateTime.UtcNow, result);
        }
    }

    public class NameEnquiryResponse
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string ChannelCode { get; set; } = string.Empty;
        public string DestinationInstitutionCode { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
    }
}