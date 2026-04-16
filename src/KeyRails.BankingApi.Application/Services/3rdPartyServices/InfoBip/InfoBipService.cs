namespace KeyRails.BankingApi.Application.ExternalServices;
public class InfoBipService(IConfiguration configuration, IGenericServices genericServices) : IInfoBipService
{
    private readonly IConfiguration configuration = configuration;
    private readonly IGenericServices genericServices = genericServices;

    public async Task<InfoBipResponse> SendSms(InfoBipRequest myRequest)
    {
        var myResponse = new InfoBipResponse();


        var apiEndPoint = this.configuration.GetSection("InfoBip:SendSmsUrl").Value;
        var authorization = this.configuration.GetSection("InfoBip:Authorization").Value;

        var jsonValue = JsonConvert.SerializeObject(myRequest);


        var headers = new Dictionary<string, string>
        {
            { "Authorization", authorization! },
            { "Content-Type", "application/json" },
            { "Accept", "application/json" }
        };


        var apiResponse = await this.genericServices.ConsumeRestAPI(apiEndPoint!, jsonValue, headers);


        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        myResponse = JsonConvert.DeserializeObject<InfoBipResponse>(apiResponse, settings);
        return myResponse!;
    }
}
