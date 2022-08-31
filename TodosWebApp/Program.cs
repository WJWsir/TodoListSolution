using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace mysqlConnectionDemo
{
    public class Program
    {
        public static int Main(string[] args)
        {
            /* 读取appsetting.json文件中对Serilog的配置数据 */
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration Configuration = builder.Build();
            string connstr = Configuration["Serilog:WriteTo:1:Args:connectionString"];
            string logTableName = Configuration["Serilog:WriteTo:1:Args:tableName"];
            LogEventLevel minimumLevel = Enum.Parse<LogEventLevel>(Configuration["Serilog:MinimumLevel"], true);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", minimumLevel)
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.MySQL(connstr, logTableName)
                .CreateBootstrapLogger(); // <-- Change this line!;

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    .MinimumLevel.Override("Microsoft", Enum.Parse<LogEventLevel>(context.Configuration["Serilog:MinimumLevel"], true))
                    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.MySQL(context.Configuration.GetConnectionString("TodoListDatabase"), context.Configuration["Serilog:WriteTo:1:Args:tableName"]))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
