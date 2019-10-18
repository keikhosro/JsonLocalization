using System;
using System.Collections.Generic;

namespace JsonLocalization
{
    public class DefaultKeyProvider : IKeyProvider
    {
        public string GetKey(IReadOnlyCollection<string> keys, string key, string locale)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (string.IsNullOrWhiteSpace(locale))
            {
                throw new ArgumentNullException(nameof(locale));
            }

            return $"{locale.Trim().ToLower()}{Constants.KeySeparator}{key.Trim()}";
        }
    }
}
