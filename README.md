# JsonLocalization
A simple .NET JSON localization library

# Usage
Set the JSON localization settings:

```c#
services.AddJsonLocalization(options => {
    options.ResourceFolders = @".\\locale"
    options.DefaultLocale = "en-US"
});
```

Create locale files at the location specified at ResourceFolders. The local files should be named in languagecode-country.json format, for example en-us.json.

Add translations in the following format:

```json
{
  "greeting": "Hello!",
  "farewell": "Goodbye!"
}
```

Inject the II18N into your constructor:

```c#
public MyClass(II18N i18n) {
    _i18n = i18n;
}
```

Perform a translation:

```c#
_i18n.Translate("greeting")
```

To change the locale:

```c#
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-CA");
```

# Fallback and substitution
You can provide a fallback string and format parameters:

```json
{
  "greeting": "Hello {0}!"
}
```

```c#
_i18n.Translate("greeting", "Hi!", "John")
```

Outputs:

```
Hello John!
```