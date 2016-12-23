namespace Suneco.SwitchingLinkProvider.Business
{
    public interface ILogger
    {
        void Info(string message, object owner);
        void Warn(string message, object owner);
        void Error(string message, object owner);
        void Debug(string message, object owner);
    }
}