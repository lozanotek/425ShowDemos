using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Desktop;
using System.Windows;

namespace Four25ShowClient
{
    public partial class App : Application
    {
        static App()
        {
            CreateApplication();
        }

        public static void CreateApplication()
        {
            var builder = ClientApplication.CreateBuilder();

            //Requires redirect URI "ms-appx-web://microsoft.aad.brokerplugin/{client_id}" in app registration
            builder.WithExperimentalFeatures();
            builder.WithWindowsBroker(true);

            if (builder == null)
            {
                return;
            }

            ClientApplication.PublicClientApp = builder.Build();
            TokenCacheHelper.EnableSerialization(ClientApplication.PublicClientApp.UserTokenCache);
        }
    }
}
