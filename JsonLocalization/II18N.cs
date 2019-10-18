namespace JsonLocalization
{
    public interface II18N
    {
        string Translate(string key, string fallback = default(string), params object[] args);
    }
}
