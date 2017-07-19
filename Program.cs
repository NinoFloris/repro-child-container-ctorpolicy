using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StructureMap;

namespace repro_child_container_ctorpolicy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var container = new Container().CreateChildContainer(); // Fails
            // var container = new Container(); // Works

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(svc =>
                {
                    svc.AddMvcCore();

                    // StructureMap.Microsoft.DependencyInjection
                    // Populate runs registry.Policies.ConstructorSelector<AspNetConstructorSelector>();
                    container.Populate(svc);
                })
                .Configure(app =>
                {
                    app.ApplicationServices = container.GetInstance<IServiceProvider>();
                    app.UseMvc();
                })
                .Build();
        }

    }
}
