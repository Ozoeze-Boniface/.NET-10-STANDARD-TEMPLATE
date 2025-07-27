using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Dtos;

namespace CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces
{
    public interface IMandateService
    {
        Task<MandateCreationResponse> CreateMandateAsync(Mandate mandate);
        Task<MandateCreationResponse> ActivateMandate(Mandate mandate);
        Task<MandateCreationResponse> GetMandateStatus(string mandateCode);
    }
}