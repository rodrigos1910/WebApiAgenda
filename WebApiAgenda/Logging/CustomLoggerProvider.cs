using System.Collections.Concurrent;

namespace WebApiAgenda.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {

        private readonly CustomLoggerProviderConfiguration _loggerConfiguration;
        private readonly ConcurrentDictionary<string, CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger>();


        public CustomLoggerProvider(CustomLoggerProviderConfiguration loggerConfiguration)
        {
            _loggerConfiguration = loggerConfiguration ?? throw new ArgumentNullException(nameof(loggerConfiguration));
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, _loggerConfiguration));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
