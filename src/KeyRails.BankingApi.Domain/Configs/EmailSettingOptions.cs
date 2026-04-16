namespace KeyRails.BankingApi.Domain.Configs;
public class EmailSettingOptions
{
    public const string EmailSetting = "EmailSetting";

    public string SmtpServer { get; set; } = "";
    public int SmtpPort { get; set; }
    public string SenderName { get; set; } = "";
    public string SenderEmail { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public string EmailTemplatePath { get; set; } = "";

}
