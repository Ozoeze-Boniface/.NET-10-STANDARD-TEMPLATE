using CityCode.MandateSystem.Application.Commands;
using CityCode.MandateSystem.Application.Common.Exceptions;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Extentions;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Application.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static CityCode.MandateSystem.Application.Extentions.ObjectExtentions;

public class MandateService : IMandateService
{
    private readonly IGenericServices _genericServices;
    private readonly NibssSettings _nibssSettings;
    private readonly SystemSettings _systemSettings;
    private readonly string _bankCode = string.Empty;

    public MandateService(IGenericServices genericServices, IOptions<SystemSettings> options)
    {
        _genericServices = genericServices;
        _nibssSettings = options.Value.NibssSettings ?? throw new ArgumentNullException(nameof(options.Value.NibssSettings));
        _bankCode = options.Value.BankCode;
        _systemSettings = options.Value;
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

    public async Task<MandateTransactionResponse> DoFundsTransfer(Mandate mandate, MandateTransactionPayload? transactionPayload = null, decimal? amount = null, bool isCharge = false)
    {
        var token = await _genericServices.LogINToNibbsFundsTransfer();
        var payload = transactionPayload ?? mandate.BuildMandateTransactionPayload(bankcode: _bankCode, amount);
        payload.BeneficiaryAccountNumber = isCharge ? _systemSettings.FeeAccountNumber : payload.BeneficiaryAccountNumber;
        payload.BeneficiaryAccountName = isCharge ? _systemSettings.FeeAccountName : payload.BeneficiaryAccountName;
        payload.BeneficiaryBankVerificationNumber = isCharge ? _systemSettings.FeeAccountBVN : payload.BeneficiaryBankVerificationNumber;
        
        var jsonPayload = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        });
        var headers = new Dictionary<string, string>()
        {
            {"Accept", "application/json"},
            {"Authorization", $"Bearer {token}"}
        };
        var response = await _genericServices.ConsumeRestAPIText(_nibssSettings.FundsTransferSettings!.TransferBaseUrl + "v1/nip/fundstransfer", jsonPayload, headers);
        var transferResponse = JsonConvert.DeserializeObject<MandateTransactionResponse>(response);

        return transferResponse ?? throw new BadRequestException("Failed to post transaction. Please try again later or contact support.");
    }

    public async Task<NameEnquiryResponse> DoNameEnquiry(DoNameEnquiryCommand command)
    {
        var token = await _genericServices.LogINToNibbsFundsTransfer();
        var payload = new 
        {
            accountNumber = command.AccountNumber,
            channelCode = command.ChannelCode,
            destinationInstitutionCode = command.DestinationInstitutionCode,
            transactionId = command.TransactionId
        };
        var jsonPayload = JsonConvert.SerializeObject(payload);
        var headers = new Dictionary<string, string>()
        {
            {"Accept", "application/json"},
            {"Authorization", $"Bearer {token}"}
        };
        var response = await _genericServices.ConsumeRestAPIText(_nibssSettings.FundsTransferSettings!.TransferBaseUrl + "v1/nip/nameenquiry", jsonPayload, headers);
        var transferResponse = JsonConvert.DeserializeObject<NameEnquiryResponse>(response);

        return transferResponse ?? throw new BadRequestException("Failed to post transaction. Please try again later or contact support.");
    }
}
