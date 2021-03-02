using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

using Microsoft.Identity.Client;

namespace Four25ShowClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private async void CallApiButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private async void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
                
        private void DisplayBasicTokenInfo(AuthenticationResult authResult)
        {
            TokenInfoText.Text = "";
            if (authResult == null)
            {
                return;
            }

            var idToken = authResult.IdToken;
            var accessToken = authResult.AccessToken;

            // Set the Id & Access Token to be displayed
            IdTokenText.Text = idToken;
            AccessTokenText.Text = accessToken;

            ClaimDisplayToken(idToken, accessToken);
        }

        private async void ClaimDisplayToken(string idToken, string accessToken)
        {
            
        }

        private static void SetThreadPrincipal(IEnumerable<Claim> claims)
        {
           
        }

        private void IdentityButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void UseWam_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SignOutButton_Click(sender, e);
            App.CreateApplication(); // Not Azure AD accounts (that is use WAM accounts)
        }
    }
}
