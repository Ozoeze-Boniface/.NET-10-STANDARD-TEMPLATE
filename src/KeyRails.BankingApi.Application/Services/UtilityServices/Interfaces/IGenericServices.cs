namespace KeyRails.BankingApi.Application.ExternalServices;

public partial interface IGenericServices
{

    Task<string> ConsumeRestAPI(string apiEndPoint, string serializedRequest, Dictionary<string, string> headers);

    Task<string> ConsumeRestAPI(string apiEndPoint, string serializedRequest, Dictionary<string, string> headers, string method);

    Task<string> ConsumeRestAPIText(string apiEndPoint, string serializedRequest, Dictionary<string, string> headers);

    Task<string> ConsumeRestAPIText(string apiEndPoint, string serializedRequest, Dictionary<string, string> headers, string method);

    Task<string> ConsumeSoapAPI(string webWebServiceUrl, string webServiceNamespace,
                                string methodVerb,
                                 string methodName,
                                 Dictionary<string, string> parameters);

    string getResponse(HttpWebRequest request);
    string GetTagContent(string mainContent, string node, string tagType);
    string GetTagContentFromJson(string mainContent, string node, string closeTag);
}
