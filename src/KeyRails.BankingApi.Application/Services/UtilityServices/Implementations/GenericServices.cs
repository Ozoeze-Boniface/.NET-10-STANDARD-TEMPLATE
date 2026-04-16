namespace KeyRails.BankingApi.Application.ExternalServices;

using System.Globalization;
using System.Text.Json.Nodes;
using KeyRails.BankingApi.Application.Common.Exceptions;
using Microsoft.Extensions.Options;

public class GenericServices : IGenericServices
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GenericServices> _logger;

    public GenericServices(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<GenericServices> logger)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<string> ConsumeRestAPI(string apiEndPoint, string serializedRequest, Dictionary<string, string> headers)
    {
        var apiResponse = "";
        var data = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

        using var httpClient = _httpClientFactory.CreateClient("NibssClient");
        using var request = new HttpRequestMessage(HttpMethod.Post, apiEndPoint);

        if (headers != null && headers.Count > 0)
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        request.Headers.TryAddWithoutValidation("Accept", "application/json");
        request.Content = data;

        var response = await httpClient.SendAsync(request);
        apiResponse = await response.Content.ReadAsStringAsync();

        return apiResponse;
    }

    public async Task<string> ConsumeRestAPI(string apiEndPoint, string serializedRequest, Dictionary<string, string> headers, string method)
    {
        var apiResponse = "";
        var data = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

        var endpointMethod = method?.ToLowerInvariant() == "get" ? HttpMethod.Get : HttpMethod.Post;

        using var httpClient = _httpClientFactory.CreateClient("NibssClient");
        using var request = new HttpRequestMessage(endpointMethod, apiEndPoint);

        if (headers != null && headers.Count > 0)
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        request.Headers.TryAddWithoutValidation("Accept", "application/json");
        request.Content = data;

        var response = await httpClient.SendAsync(request);
        apiResponse = await response.Content.ReadAsStringAsync();

        return apiResponse;
    }

    public async Task<string> ConsumeRestAPIText(string apiEndPoint, string serializedRequest, Dictionary<string, string> headers)
    {
        var apiResponse = "";
        _logger.LogInformation("NIBBS REQUEST: {ApiRequest}", serializedRequest);
        var data = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

        using var httpClient = _httpClientFactory.CreateClient("NibssClient");
        using var request = new HttpRequestMessage(HttpMethod.Post, apiEndPoint);

        if (headers != null && headers.Count > 0)
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        request.Headers.TryAddWithoutValidation("Accept", "*/*");
        request.Content = data;

        var response = await httpClient.SendAsync(request);
        apiResponse = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("NIBBS RESPONSE: {ApiResponse}", apiResponse);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(apiResponse);
        }

        return apiResponse;
    }

    public async Task<string> ConsumeRestAPIText(string apiEndPoint, string serializedRequest, Dictionary<string, string> headers, string method)
    {
        var apiResponse = "";
        var data = new StringContent("\"" + serializedRequest + "\"", Encoding.UTF8, "application/json");

        var endpointMethod = method?.ToLowerInvariant() == "get" ? HttpMethod.Get : HttpMethod.Post;

        using var httpClient = _httpClientFactory.CreateClient("NibssClient");
        using var request = new HttpRequestMessage(endpointMethod, apiEndPoint);

        if (headers != null && headers.Count > 0)
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        request.Headers.TryAddWithoutValidation("Accept", "*/*");
        request.Content = data;

        var response = await httpClient.SendAsync(request);
        apiResponse = await response.Content.ReadAsStringAsync();

        return apiResponse;
    }

    public async Task<string> ConsumeSoapAPI(string webWebServiceUrl, string webServiceNamespace,
                                 string methodVerb,
                                 string methodName,
                                 Dictionary<string, string> parameters)
    {
        var apiResponse = "";

        const string soapTemplate =
               @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
                        xmlns:{0}=""{2}"">
            <soapenv:Header />
            <soapenv:Body>
                <{0}:{1}>
                    {3}
                </{0}:{1}>
            </soapenv:Body>
        </soapenv:Envelope>";

        string parametersText;

        if (parameters != null && parameters.Count > 0)
        {
            var sb = new StringBuilder();
            foreach (var oneParameter in parameters)
            {
                sb.AppendFormat(CultureInfo.CurrentCulture, "  <{0}>{1}</{0}>\r\n", oneParameter.Key, oneParameter.Value);
            }
            parametersText = sb.ToString();
        }
        else
        {
            parametersText = "";
        }

        var soapText = string.Format(CultureInfo.CurrentCulture, soapTemplate,
                        methodVerb, methodName, webServiceNamespace, parametersText);

        var data = new StringContent(soapText, Encoding.UTF8, "text/xml");

        using var httpClient = _httpClientFactory.CreateClient("NibssClient");
        using var request = new HttpRequestMessage(HttpMethod.Post, webWebServiceUrl);

        request.Content = data;
        var response = await httpClient.SendAsync(request);
        apiResponse = await response.Content.ReadAsStringAsync();

        return apiResponse;
    }

    public string getResponse(HttpWebRequest request)
    {
        string receivedResponse;
        try
        {
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();

            var responseStream = new StreamReader(response.GetResponseStream(), Encoding.Default);
            receivedResponse = responseStream.ReadToEnd();

            return receivedResponse;
        }
        catch (Exception ex)
        {
            return receivedResponse = ex.Message;
        }
    }

    public string GetTagContent(string mainContent, string node, string tagType)
    {
        var tagContent = "NoDataFoundIbc";
        try
        {
            var openTag = "";
            var closeTag = "";

            if (tagType == "0")
            {
                openTag = "&lt;" + node + "&gt;";
                closeTag = "&lt;/" + node + "&gt;";
            }
            else
            {
                openTag = "<" + node + ">";
                closeTag = "</" + node + ">";
            }

            var startIndex = mainContent.IndexOf(openTag, StringComparison.Ordinal) + openTag.Length;

            if (startIndex != 0)
            {
                var closeTagIndex = mainContent.IndexOf(closeTag, StringComparison.Ordinal);
                var endIndex = mainContent.Length - (startIndex + mainContent[closeTagIndex..].Length);
                tagContent = mainContent.Substring(startIndex, endIndex);
            }
        }
        catch
        {
            tagContent = "-999-";
        }

        return tagContent;
    }

    public string GetTagContentFromJson(string mainContent, string node, string closeTag)
    {
        var tagContent = "NoDataFoundIbc";
        try
        {
            var openTag = node;
            var startIndex = mainContent.IndexOf(openTag, StringComparison.Ordinal) + 3 + node.Length;
            var remainingString = mainContent[startIndex..];

            if (startIndex != 0)
            {
                var closeTagIndex = remainingString.IndexOf(closeTag, StringComparison.Ordinal) - 1;
                tagContent = remainingString[..closeTagIndex];
            }
        }
        catch
        {
            tagContent = "-999-";
        }

        return tagContent;
    }
}