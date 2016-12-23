namespace Suneco.SwitchingLinkProvider.Services
{
    using Sitecore.Diagnostics;
    using Suneco.SwitchingLinkProvider.Services.Interfaces;

    /// <summary>
    /// Provides access to the sitecore logging methods.
    /// </summary>
    /// <seealso cref="Suneco.SwitchingLinkProvider.Services.Interfaces.ILoggingService" />
    public class SitecoreLogging : ILoggingService
    {
        /// <summary>
        /// Logs the specified information message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner.</param>
        public void Info(string message, object owner)
        {
            Log.Warn(message, owner);
        }

        /// <summary>
        /// Logs the specified warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner.</param>
        public void Warn(string message, object owner)
        {
            Log.Warn(message, owner);
        }

        /// <summary>
        /// Logs the specified error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner.</param>
        public void Error(string message, object owner)
        {
            Log.Warn(message, owner);
        }

        /// <summary>
        /// Logs the specified debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner.</param>
        public void Debug(string message, object owner)
        {
            Log.Warn(message, owner);
        }
    }
}