namespace CityCode.MandateSystem.Domain.Configs;
public class DatabaseSettingOptions
{
    public const string DatabaseSetting = "DatabaseSetting";

    public string ConnectionString { get; set; } = "";
    public string DatabaseName { get; set; } = "";
    public string DatabaseType { get; set; } = "";

}
