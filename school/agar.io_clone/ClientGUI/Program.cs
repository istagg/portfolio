using FileLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI {
   public static class Program {


      public static void Main(string[] args) {
            //var services = new ServiceCollection();
            //ConfigureServices(services);

            //using ServiceProvider serviceProvider = services.BuildServiceProvider();
            //var gui = serviceProvider.GetRequiredService<ClientGui>();
            Application.Run(new ClientGui());
      }

      /// <summary>
      /// A helper method to configure our services.
      /// </summary>
      /// <param name="services">Services to configure</param>
      private static void ConfigureServices(ServiceCollection services) {
         services.AddLogging(configure => {
            configure.AddConsole();
            configure.AddDebug();
            configure.AddProvider(new CustomFileLogProvider());
            configure.SetMinimumLevel(LogLevel.Debug);
         });
         services.AddScoped<ClientGui>();
      }
   }
}

