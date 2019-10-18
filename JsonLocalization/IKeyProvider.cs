using System.Collections.Generic;

namespace JsonLocalization
{
    public interface IKeyProvider
    {
        string GetKey(IReadOnlyCollection<string> keys, string key, string locale);
    }
}
