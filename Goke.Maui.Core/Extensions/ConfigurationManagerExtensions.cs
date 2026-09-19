using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Goke.Maui.Core.Extensions;

public static class ConfigurationManagerExtensions
{
    public static IConfigurationManager AddEmbeddedJsonResource(
        this IConfigurationManager configuration,
        Assembly assembly,
        string resourceName)
    {
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Could not find embedded resource '{resourceName}'.");

        configuration.AddJsonStream(stream);
        return configuration;

    }
}


