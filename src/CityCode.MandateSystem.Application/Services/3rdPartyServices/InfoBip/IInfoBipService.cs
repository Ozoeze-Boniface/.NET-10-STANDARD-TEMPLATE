namespace CityCode.MandateSystem.Application.ExternalServices;
public partial interface IInfoBipService
{
    Task<InfoBipResponse> SendSms(InfoBipRequest myRequest);
}
