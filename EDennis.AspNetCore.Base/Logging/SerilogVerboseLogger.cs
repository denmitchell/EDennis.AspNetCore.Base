using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Logging
{

    /// <summary>
    /// A Serilog logger that log that logs at Verbose and higher.  The configuration
    /// is in "Logging:Loggers:SerilogVerboseLogger".
    /// 
    /// Note: UseSerilog() must be specified in Program.cs
    /// Note: Startup.cs ConfigureServices must have the following:
    ///<code>
    /// services.AddScoped<ScopeProperties>();
    /// services.AddSingleton(typeof(ILogger<>), typeof(SerilogVerboseLogger<>));
    /// services.AddSingleton<ILoggerChooser>(f => {
    ///    var loggers = f.GetRequiredService<IEnumerable<ILogger<object>>>();
    ///    return new DefaultLoggerChooser(loggers); // or use your own LoggerChooser impl.
    /// });
    ///</code>                       
    /// Note: Classes must inject both ScopeProperties and <code>IEnumerable<ILogger<T>></code>
    ///       as per this example:
    /// <code>
    /// ILogger<SomeClass> _logger;
    /// public SomeClass(IEnumerable<ILogger<SomeClass>> loggers,
    ///                  ScopeProperties scopeProperties) {
    ///        _logger = loggers.ElementAt(scopeProperties.LoggerIndex);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T">Class into which the logger is injected</typeparam>
    public class SerilogVerboseLogger<T> : CustomSerilogLogger<T>
    {
        public SerilogVerboseLogger(ILoggerFactory factory, IConfiguration configuration) : base(factory, configuration) { }
    }
}
