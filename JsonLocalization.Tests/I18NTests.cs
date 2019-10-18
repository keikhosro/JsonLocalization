using Microsoft.Extensions.Options;
using Xunit;

namespace JsonLocalization.Tests
{
    public class I18NTests
    {
        [Theory]
        [InlineData("Fallback", @"{""hello"":""Hello World!""}", "bye", "Fallback", null)]
        [InlineData("Hello World!", @"{""hello"":""Hello World!""}", "hello", null, null)]
        [InlineData("Hello John!", @"{""hello"":""Hello {0}!""}", "hello", null, new[] { "John" })]
        public void Translate(string expected, string json, string key, string fallback, string[] args)
        {
            var options = new JsonLocalizationOptions
            {
                ResourceFolders = new[] { "test" },
                DefaultLocale = "en-CA"
            };

            DefaultDictionaryBuilder.ReadAllText = path => json;
            DefaultDictionaryBuilder.DirectoryGetFiles = (path, searchPattern, searchOption) => new[] { "en-ca.json" };

            I18N.BuildDictionary(new DefaultDictionaryBuilder(), options);

            var sut = new I18N(new OptionsWrapper<JsonLocalizationOptions>(options), new DefaultKeyProvider());

            Assert.Equal(expected, sut.Translate(key, fallback, args));
        }
    }
}
