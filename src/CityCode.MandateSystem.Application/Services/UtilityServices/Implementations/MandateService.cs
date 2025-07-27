using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Exceptions;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Extentions;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;

namespace CityCode.MandateSystem.Application.Services.UtilityServices
{
    public class MandateService(IGenericServices genericServices) : IMandateService
    {
        private readonly IGenericServices _genericServices = genericServices;

        public async Task<MandateCreationResponse> CreateMandateAsync(Mandate mandate)
        {
            var token = await _genericServices.LogINToNibbs();

            // CALL NIBBS AUTHENTICATION API
            var headers = new Dictionary<string, string>()
            {
                {"Accept", "application/json"},
                {"Authorization", token}
            };
            var payload = mandate.CreateMandatePayload();
            var createMandateEndpoint = "https://apitest.nibss-plc.com.ng/ndd/api/MandateRequest/CreateEmandate";
            var createMandateResponse = await _genericServices.ConsumeRestAPIText(createMandateEndpoint, payload, headers);

            var mandateResponse = JsonConvert.DeserializeObject<MandateCreationResponse>(createMandateResponse);
            return mandateResponse ?? throw new BadRequestException("Failed to create mandate. Please try again later or contact support.");

        }

        public async Task<MandateCreationResponse> ActivateMandate(Mandate mandate)
        {
            var token = await _genericServices.LogINToNibbs();
            var mandatePayload = mandate.FormaMandateActivationPayload();
            // CALL NIBBS AUTHENTICATION API
            var headers = new Dictionary<string, string>()
            {
                {"Accept", "application/json"},
                {"Authorization", token}
            };
            var payload = JsonConvert.SerializeObject(mandatePayload);
            var activateMandateResponse = "https://apitest.nibss-plc.com.ng/ndd/api/MandateRequest/UpdateMandateStatus";
            var createMandateResponse = await _genericServices.ConsumeRestAPIText(activateMandateResponse, payload, headers);

            var mandateResponse = JsonConvert.DeserializeObject<MandateCreationResponse>(createMandateResponse);
            return mandateResponse ?? throw new BadRequestException("Failed to create mandate. Please try again later or contact support.");
        }
        
        public async Task<MandateCreationResponse> GetMandateStatus(string mandateCode)
        {
            var token = await _genericServices.LogINToNibbs();
            // CALL NIBBS AUTHENTICATION API
            var headers = new Dictionary<string, string>()
            {
                {"Accept", "application/json"},
                {"Authorization", token}
            };
            var mandateStatusEndpoint = $"https://apitest.nibss-plc.com.ng/ndd/api/MandateRequest/MandateStatus?MandateCode={mandateCode}";
            var createMandateResponse = await _genericServices.ConsumeRestAPIText(mandateStatusEndpoint, mandateStatusEndpoint, headers);

            var mandateResponse = JsonConvert.DeserializeObject<MandateCreationResponse>(createMandateResponse);
            return mandateResponse ?? throw new BadRequestException("Failed to create mandate. Please try again later or contact support.");
        }
    }
}