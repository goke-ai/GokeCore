namespace Goke.Core.Options;

public sealed class CookieAuthOptions
{
    public const string SectionName = "Authentication:Cookie";

    public string LoginPath { get; set; } = "/Account/Login";
    public string AccessDeniedPath { get; set; } = "/Account/AccessDenied";
    public bool SlidingExpiration { get; set; } = true;
    public int ExpireTimeSpanMinutes { get; set; } = 60;
}