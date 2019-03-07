namespace Syrus.Plugin
{
    public interface IAppApi
    {
        void ChangeQuery(string query, bool append = false);
        void ChangeQuickResult(string text);
        void SetHelpPlaceholder(string text);
    }
}
