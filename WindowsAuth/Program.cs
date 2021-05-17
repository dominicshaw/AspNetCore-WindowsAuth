using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Serilog;
using Serilog.Events;

namespace WindowsAuth
{
    public class Program
    {
        private static string LogFileLocation() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Logs", "WinAuth");

        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Debug)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Debug)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Debug)
                .MinimumLevel.Override("Microsoft.AspNetCore.Routing.EndpointMiddleware", LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}")
                .WriteTo.File(Path.Combine(LogFileLocation(), @"Win-Auth-.txt"), LogEventLevel.Debug, rollingInterval: RollingInterval.Month)
                .CreateLogger();

            try
            {
                Log.Information("Starting WinAuth");

                var isService = !(Debugger.IsAttached || args.Contains("--console"));

                if (isService)
                {
                    Log.Error(LogFileLocation());

                    using var currentProcess = Process.GetCurrentProcess();

                    var pathToExe = currentProcess.MainModule?.FileName;
                    var pathToContentRoot = Path.GetDirectoryName(pathToExe);

                    Directory.SetCurrentDirectory(pathToContentRoot!);
                }

                var builder = CreateHostBuilder(args.Where(arg => arg != "--console").ToArray());

                using (var host = builder.Build())
                {
                    if (isService)
                        host.RunAsService();
                    else
                        host.Run();
                }

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://*:3278")
                .UseSerilog();
        }
    }
}
