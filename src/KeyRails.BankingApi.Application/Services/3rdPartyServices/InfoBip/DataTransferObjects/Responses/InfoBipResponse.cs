namespace KeyRails.BankingApi.Application.ExternalServices;
public class Status
{
    public string? description { get; set; }
    public int? groupId { get; set; }
    public string? groupName { get; set; }
    public int? id { get; set; }
    public string? name { get; set; }
}

public class ResponseMessage
{
    public string? messageId { get; set; }
    public Status? status { get; set; }
    public string? to { get; set; }
}

public class InfoBipResponse
{
    public string? bulkId { get; set; }
    public List<ResponseMessage>? messages { get; set; }
}

