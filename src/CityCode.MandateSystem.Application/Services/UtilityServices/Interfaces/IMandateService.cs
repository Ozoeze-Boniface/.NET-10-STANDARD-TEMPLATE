using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Commands;
using CityCode.MandateSystem.Application.Dtos;

namespace CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces
{
    public interface IMandateService
    {
        Task<MandateCreationResponse> CreateMandateAsync(Mandate mandate);
        Task<MandateCreationResponse> ActivateMandate(Mandate mandate);
        Task<MandateCreationResponse> GetMandateStatus(string mandateCode);
        Task<MandateTransactionResponse> DoFundsTransfer(Mandate mandate);
        Task<NameEnquiryResponse> DoNameEnquiry(DoNameEnquiryCommand command);
    }
}