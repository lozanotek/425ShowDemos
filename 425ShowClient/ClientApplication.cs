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
            return null;
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
