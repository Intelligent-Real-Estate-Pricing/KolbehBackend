using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

public static class SerilogConfig
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
        (hostingContext, loggerConfiguration) =>
        {
            var env = hostingContext.HostingEnvironment;
            var applicationName = env.ApplicationName;
            var environmentName = env.EnvironmentName;

            var logFolder = "/tmp/logs";
            Directory.CreateDirectory(logFolder);

            var logFilePath = Path.Combine(logFolder, "log-.txt");

            loggerConfiguration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.WithProperty("ApplicationName", applicationName)
                .Enrich.WithProperty("EnvironmentName", environmentName)
                .WriteTo.Console()
                .WriteTo.File(
                    logFilePath,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                    shared: true
                );
        };
}
