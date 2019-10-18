using System;
using Microsoft.Extensions.DependencyInjection;

namespace JsonLocalization.Web.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonLocalization(this IServiceCollection services,
            Action<JsonLocalizationOptions> setupAction)
        {
            return AddJsonLocalization(services, new DefaultDictionaryBuilder(), setupAction);
        }

        public static IServiceCollection AddJsonLocalization(this IServiceCollection services,
            IDictionaryBuilder dictionaryBuilder, Action<JsonLocalizationOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (dictionaryBuilder == null)
            {
                throw new ArgumentNullException(nameof(dictionaryBuilder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            void SetupWrapperAction(JsonLocalizationOptions options)
            {
                setupAction(options);
                I18N.BuildDictionary(dictionaryBuilder, options);
            }

            services.Configure<JsonLocalizationOptions>(SetupWrapperAction);
            services.AddScoped<II18N, I18N>();
            services.AddScoped<IKeyProvider, DefaultKeyProvider>();

            return services;
        }
    }
}
