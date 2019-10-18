using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JsonLocalization.Tests")]

namespace JsonLocalization
{
    public class DefaultDictionaryBuilder : IDictionaryBuilder
    {
        public virtual void Build(ConcurrentDictionary<string, string> dictionary, JsonLocalizationOptions options)
        {
            if (options.ResourceFolders == null)
            {
                throw new ArgumentNullException(nameof(options.ResourceFolders));
            }

            if (string.IsNullOrEmpty(options.LocaleFileExtension))
            {
                throw new ArgumentNullException(nameof(options.LocaleFileExtension));
            }

            foreach (var path in options.ResourceFolders)
            {
                foreach (var file in DirectoryGetFiles(path, $"*.{options.LocaleFileExtension}",
                    SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        var locale = Path.GetFileNameWithoutExtension(file)?.ToLower();
                        if (string.IsNullOrWhiteSpace(locale))
                        {
                            continue;
                        }

                        var jobj = JObject.Parse(ReadAllText(file));
                        foreach (var property in jobj.Properties())
                        {
                            var value = property.Value?.ToString();
                            var cacheKey = GetCacheKey(path, file, property.Name);
                            dictionary.AddOrUpdate(cacheKey, value, (k, oldValue) => value);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        public virtual string GetCacheKey(string path, string file, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var locale = Path.GetFileNameWithoutExtension(file)?.Trim().ToLower();
            return $"{locale}{Constants.KeySeparator}{key?.Trim()}";
        }

        internal static Func<string, string> ReadAllText = File.ReadAllText;

        internal static Func<string, string, SearchOption, string[]> DirectoryGetFiles = Directory.GetFiles;
    }
}