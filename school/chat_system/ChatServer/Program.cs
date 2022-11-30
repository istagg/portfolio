using FileLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChatServer {
    internal static class Program {
      /// <summary>
      ///  The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main() {
         var services = new ServiceCollection();
         ConfigureServices(services);

         using ServiceProvider serviceProvider = services.BuildServiceProvider();
         var gui = serviceProvider.GetRequiredService<ChatServer>();
         Application.Run(gui);
      }

      /// <summary>
      /// Configures the logger to work with our <see cref="CustomFileLogProvider"/> and <see cref="CustomFileLogger"/>.
      /// </summary>
      /// <param name="services">A service collection to attach the logger to.</param>
      private static void ConfigureServices(ServiceCollection services) {
         services.AddLogging(configure => {
            configure.AddConsole();
            configure.AddDebug();
            configure.AddProvider(new CustomFileLogProvider());
            configure.SetMinimumLevel(LogLevel.Debug);
         });
         services.AddScoped<ChatServer>();
      }
   }
}