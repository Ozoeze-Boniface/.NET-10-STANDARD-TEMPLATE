namespace KeyRails.BankingApi.Application.ExternalServices;
public class Destination
{
    public string? to { get; set; }
}
public class Message
{
    public string? from { get; set; }
    public List<Destination>? destinations { get; set; }
    public string? text { get; set; }
}

public class InfoBipRequest
{
    public List<Message>? messages { get; set; }
}
