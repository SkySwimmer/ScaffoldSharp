
namespace ScaffoldSharp.Logging
{

    /// <summary>
    /// Logger implementation provider
    /// </summary>
    public interface ILoggerImplementationProvider
    {
        /// <summary>
        /// Creates a logger instance
        /// </summary>
        /// <param name="name">Logger name</param>
        /// <returns>New Logger instance</returns>
        public Logger CreateInstance(string name);
    }

}