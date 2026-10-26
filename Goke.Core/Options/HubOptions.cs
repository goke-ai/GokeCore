namespace Goke.Core.Options;

public sealed class HubOptions
{
    public const string SectionName = "Hubs";
    public string BaseUrl { get; set; } = string.Empty;
    public List<HubOption> Connections { get; set; } = new List<HubOption>();
}


public class HubOption
{
    public string HubName { get; set; } = "chat";
    public string AuthProvider { get; set; } = "Cookie";
    public string Path { get; set; } = "/hubs/chat";
}