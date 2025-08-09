using CityCode.MandateSystem.Application.Common.Exceptions;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Extentions;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Application.Settings;
using Microsoft.Extensions.Options;

public class MandateService : IMandateService
{
    private readonly IGenericServices _genericServices;
    private readonly NibssSettings _nibssSettings;

    public MandateService(IGenericServices genericServices, IOptions<SystemSettings> options)
    {
        _genericServices = genericServices;
        _nibssSettings = options.Value.NibssSettings ?? throw new ArgumentNullException(nameof(options.Value.NibssSettings));
    }

    public async Task<MandateCreationResponse> CreateMandateAsync(Mandate mandate)
    {
        var token = await _genericServices.LogINToNibbs();

        var headers = new Dictionary<string, string>()
        {
            {"Accept", "application/json"},
            {"Authorization", $"Bearer {token}"}
        };
        var payload = mandate.CreateMandatePayload();
        var createMandateResponse = await _genericServices.ConsumeRestAPIText(_nibssSettings.CreateMandateUrl, payload, headers);

        var mandateResponse = JsonConvert.DeserializeObject<MandateCreationResponse>(createMandateResponse);
        return mandateResponse ?? throw new BadRequestException("Failed to create mandate. Please try again later or contact support.");
    }

    public async Task<MandateCreationResponse> ActivateMandate(Mandate mandate)
    {
        var token = await _genericServices.LogINToNibbs();
        var mandatePayload = mandate.FormaMandateActivationPayload();

        var headers = new Dictionary<string, string>()
        {
            {"Accept", "application/json"},
            {"Authorization", $"Bearer {token}"}
        };
        var createMandateResponse = await _genericServices.ConsumeRestAPIText(_nibssSettings.UpdateMandateStatusUrl, mandatePayload, headers);

        var mandateResponse = JsonConvert.DeserializeObject<MandateCreationResponse>(createMandateResponse);
        return mandateResponse ?? throw new BadRequestException("Failed to activate mandate. Please try again later or contact support.");
    }
    
    public async Task<MandateCreationResponse> GetMandateStatus(string mandateCode)
    {
        var token = await _genericServices.LogINToNibbs();
        
        var headers = new Dictionary<string, string>()
        {
            {"Accept", "application/json"},
            {"Authorization", $"Bearer {token}"}
        };
        var mandateStatusEndpoint = $"{_nibssSettings.MandateStatusUrl}?MandateCode={mandateCode}";
        var createMandateResponse = await _genericServices.ConsumeRestAPIText(mandateStatusEndpoint, mandateStatusEndpoint, headers);

        var mandateResponse = JsonConvert.DeserializeObject<MandateCreationResponse>(createMandateResponse);
        return mandateResponse ?? throw new BadRequestException("Failed to get mandate status. Please try again later or contact support.");
    }
}
