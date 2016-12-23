namespace Suneco.SwitchingLinkProvider.Business
{
    using Sitecore.Diagnostics;

    public class SitecoreLogger : ILogger
    {
        public void Info(string message, object owner)
        {
            Log.Warn(message, owner);
        }

        public void Warn(string message, object owner)
        {
            Log.Warn(message, owner);
        }

        public void Error(string message, object owner)
        {
            Log.Warn(message, owner);
        }

        public void Debug(string message, object owner)
        {
            Log.Warn(message, owner);
        }
    }
}