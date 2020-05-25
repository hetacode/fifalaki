using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace GamesList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((c, o) =>
                        {
                            if (c.Configuration["ReleaseMode"] == "dev")
                            {
                                o.ListenLocalhost(5001, listen => listen.Protocols = HttpProtocols.Http2);
                            }
                            else
                            {
                                o.ListenAnyIP(5001, listen => listen.Protocols = HttpProtocols.Http2);
                            }

                        })
                        .UseStartup<Startup>();
                });
    }
}
