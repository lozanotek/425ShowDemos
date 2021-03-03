using System.Configuration;
using System.Globalization;

using Microsoft.Identity.Client;

namespace Four25ShowClient
{
    public static class ClientApplication
    {
        public static IPublicClientApplication PublicClientApp { get; set; }

        public static PublicClientApplicationBuilder CreateBuilder()
        {
            var clientId = GetConfigValue(Constants.ClientIdKey);
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return null;
            }

            var instance = GetConfigValue(Constants.ADInstanceKey);
            var tenant = GetConfigValue(Constants.TenantKey);

            var authority = string.Format(CultureInfo.InvariantCulture, instance, tenant);

            var builder = PublicClientApplicationBuilder.Create(clientId)
               .WithAuthority(authority)
               .WithDefaultRedirectUri();

            return builder;
        }

        public static string[] GetAppScopes()
        {
            return new[] { GetConfigValue(Constants.ApiScopeKey) };
        }

        public static string GetApiAddress()
        {
            return GetConfigValue(Constants.ApiAddressKey);
        }

        private static string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
