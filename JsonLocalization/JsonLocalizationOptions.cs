namespace JsonLocalization
{
    public class JsonLocalizationOptions
    {
        public string[] ResourceFolders { get; set; }

        public string DefaultLocale { get; set; }

        public string LocaleFileExtension { get; set; } = "json";
    }
}
