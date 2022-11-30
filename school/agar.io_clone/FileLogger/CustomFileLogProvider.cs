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
/// This file creates the provider for our custom file logger.
/// </summary>

using Microsoft.Extensions.Logging;

namespace FileLogger {

    /// <summary>
    /// Creates the provider for custom file logger
    /// </summary>
    public class CustomFileLogProvider : ILoggerProvider {
        private CustomFileLogger _logger;

        /// <summary>
        /// Creates a new custom file logger.
        /// </summary>
        /// <returns>Custom File Logger</returns>
        public ILogger CreateLogger(string categoryName) {
            _logger = new CustomFileLogger(categoryName);
            return _logger;
        }

        /// <summary>
        /// Disposes the file logger
        /// </summary>
        public void Dispose() {
            _logger.Dispose();
        }
    }
}