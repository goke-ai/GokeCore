namespace Goke.Maui.Core.Services;

public static class HttpClientHelper
{
    public static HttpMessageHandler CreatePlatformMessageHandler()
    {
#if WINDOWS || MACCATALYST
        return new HttpClientHandler();
#else
        return new HttpsClientHandlerService().PlatformMessageHandler;
#endif
    }
}

internal sealed class HttpsClientHandlerService
{
    public HttpMessageHandler PlatformMessageHandler
    {
        get
        {
#if ANDROID
            var handler = new Xamarin.Android.Net.AndroidMessageHandler();
            handler.ServerCertificateCustomValidationCallback = (_, cert, _, errors) =>
            {
                if (cert is not null && cert.Issuer.Equals("CN=localhost", StringComparison.Ordinal))
                {
                    return true;
                }

                return errors == System.Net.Security.SslPolicyErrors.None;
            };

            return handler;
#elif IOS
            return new NSUrlSessionHandler
            {
                TrustOverrideForUrl = IsHttpsLocalhost
            };
#else
            throw new PlatformNotSupportedException("Only Android and iOS supported.");
#endif
        }
    }

#if IOS
    private static bool IsHttpsLocalhost(NSUrlSessionHandler _, string url, Security.SecTrust __)
    {
        return url.StartsWith("https://localhost", StringComparison.OrdinalIgnoreCase);
    }
#endif
}