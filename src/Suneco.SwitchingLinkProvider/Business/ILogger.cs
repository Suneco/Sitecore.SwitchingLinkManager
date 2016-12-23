namespace Suneco.SwitchingLinkProvider.Business
{
    /// <summary>
    /// Interface that provides access to logging methods.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the specified information message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner.</param>
        void Info(string message, object owner);

        /// <summary>
        /// Logs the specified warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner.</param>
        void Warn(string message, object owner);

        /// <summary>
        /// Logs the specified error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner.</param>
        void Error(string message, object owner);

        /// <summary>
        /// Logs the specified debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner.</param>
        void Debug(string message, object owner);
    }
}