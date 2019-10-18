using System.Collections.Concurrent;

namespace JsonLocalization
{
    public interface IDictionaryBuilder
    {
        void Build(ConcurrentDictionary<string, string> dictionary, JsonLocalizationOptions options);

        string GetCacheKey(string path, string file, string key);
    }
}
