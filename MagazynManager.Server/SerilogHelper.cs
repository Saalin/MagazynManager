using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.IO;

namespace MagazynManager.Server
{
    public static class SerilogHelper
    {
        public static void ConfigureSerilog()
        {
            var config = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext();

            Log.Logger = config
                .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log.log"), rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                .WriteTo.File(new CompactJsonFormatter(), Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log.json"), rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                .CreateLogger();

            Log.Information("Successfully created logger");
        }
    }
}