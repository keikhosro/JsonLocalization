using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("JsonLocalization.Tests")]

namespace JsonLocalization
{
    public class I18N : II18N
    {
        private static readonly ConcurrentDictionary<string, string> Translations =
            new ConcurrentDictionary<string, string>();

        private readonly JsonLocalizationOptions _options;
        private readonly IKeyProvider _keyProvider;

        public I18N(
            IOptions<JsonLocalizationOptions> options,
            IKeyProvider keyProvider
        )
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _keyProvider = keyProvider ?? throw new ArgumentNullException(nameof(keyProvider));
        }

        public string Translate(string key, string fallback = null, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return fallback;
            }

            if (string.IsNullOrWhiteSpace(_options.DefaultLocale))
            {
                throw new ArgumentNullException(nameof(_options.DefaultLocale));
            }

            var cacheKey = _keyProvider.GetKey(new ReadOnlyCollection<string>(Translations.Keys.ToList()), key.Trim(),
                CurrentLocale);
            return Translations.ContainsKey(cacheKey)
                ? string.Format(Translations[cacheKey], args ?? new object[0])
                : fallback;
        }

        public static void BuildDictionary(IDictionaryBuilder dictionaryBuilder, JsonLocalizationOptions options)
        {
            if (dictionaryBuilder == null)
            {
                throw new ArgumentNullException(nameof(dictionaryBuilder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            ClearDictionary();

            dictionaryBuilder.Build(Translations, options);
        }

        public static void ClearDictionary()
        {
            Translations.Clear();
        }

        protected string CurrentLocale => !string.IsNullOrEmpty(Thread.CurrentThread.CurrentCulture.Name)
            ? Thread.CurrentThread.CurrentCulture?.Name
            : _options.DefaultLocale;
    }
}