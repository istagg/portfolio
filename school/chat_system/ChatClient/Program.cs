/// <summary>
/// Author:    Isaac Stagg
/// Partner:   Nate Tripp
/// Date:      4/2/2022
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Isaac Stagg and Nate Tripp - This work may not be copied for use in Academic Coursework.
///
/// I, Isaac Stagg and Nate Tripp, certify that I wrote this code from scratch and did not copy it in part or whole from
/// another source.  All references used in the completion of the assignment are cited in my README file.
///
/// File Contents
/// This file contains code for configuring services for the client.
/// </summary>

using FileLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChatClient {

    /// <summary>
    /// This class sets up and configures the services required for the chat client.
    /// </summary>
    internal static class Program {

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            var services = new ServiceCollection();
            ConfigureServices(services);

            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            var gui = serviceProvider.GetRequiredService<ChatClient>();
            Application.Run(gui);
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
            services.AddScoped<ChatClient>();
        }
    }
}