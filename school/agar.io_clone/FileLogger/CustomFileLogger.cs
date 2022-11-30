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
/// This file contains the custom file logger. Here we set up the file that we are writing to and commit messages to it.
/// </summary>

using Microsoft.Extensions.Logging;

namespace FileLogger {

    /// <summary>
    /// Class that handles logging messages to a custom file.
    /// </summary>
    public class CustomFileLogger : ILogger, IDisposable {
        public const string FILE_NAME = "Log_CS3500_Assignment8.log";

        private readonly string _category;
        private readonly StreamWriter _stream;

        /// <summary>
        /// Constructor for the custom file logger, opens a file to write log entries to.
        /// </summary>
        /// <param name="category">Required by the provider</param>
        internal CustomFileLogger(string category) {
            _category = category;
            _stream = new StreamWriter(FILE_NAME, true);
        }

        /// <inheritdoc/>
        public IDisposable BeginScope<TState>(TState state) {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool IsEnabled(LogLevel logLevel) {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
            var msg = $"{DateTime.Now:G} ({Environment.CurrentManagedThreadId}) - {logLevel.ToString()[..5]} - {formatter(state, exception)}\n";
            lock (_category) {
                _stream.Write(msg);
            }
        }

        /// <inheritdoc/>
        public void Dispose() {
            _stream.Flush();
            _stream.Dispose();
        }
    }
}