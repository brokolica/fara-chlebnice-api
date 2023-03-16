using System.Reflection;

namespace Api.ConfigLoader;

public static class ConfigurationLoader
{
    public static IConfigurationBuilder LoadConfigurations(
        this IConfigurationBuilder config,
        string? environmentName = null)
    {
        var configsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        config
            .SetBasePath(configsPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        if (!string.IsNullOrWhiteSpace(environmentName))
        {
            config.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
        }

        config.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

        return config;
    }
}