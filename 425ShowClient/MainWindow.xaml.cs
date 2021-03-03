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
            AuthenticationResult authResult = null;
            var app = ClientApplication.PublicClientApp;

            if (app == null)
            {
                return;
            }

            var scopes = ClientApplication.GetAppScopes();

            ResultText.Text = string.Empty;
            TokenInfoText.Text = string.Empty;

            IAccount firstAccount;

            switch (howToSignIn.SelectedIndex)
            {
                // 0: Use account used to signed-in in Windows (WAM)
                case 0:

                    firstAccount = PublicClientApplication.OperatingSystemAccount;
                    break;
                //  1: Use one of the Accounts known by Windows(WAM)
                case 1:
                    firstAccount = null;
                    break;

                default:
                    var accounts = await app?.GetAccountsAsync();
                    firstAccount = accounts.FirstOrDefault();
                    break;
            }

            try
            {
                authResult = await app.AcquireTokenSilent(scopes, firstAccount).ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilent. 
                // This indicates you need to call AcquireTokenInteractive to acquire a token
                System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try
                {
                    authResult = await app.AcquireTokenInteractive(scopes)
                        .WithAccount(firstAccount)
                        .WithParentActivityOrWindow(new WindowInteropHelper(this).Handle) // optional, used to center the browser on the window
                        .WithPrompt(Prompt.SelectAccount)
                        .ExecuteAsync();
                }
                catch (MsalException msalex)
                {
                    ResultText.Text = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                return;
            }

            if (authResult != null)
            {
                DisplayBasicTokenInfo(authResult);

                this.SignOutButton.Visibility = Visibility.Visible;
                this.IdentityButton.Visibility = Visibility.Visible;
            }
        }

        private async void CallApiButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private async void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            var clientApp = ClientApplication.PublicClientApp;
            if (clientApp == null)
            {
                return;
            }

            var accounts = await clientApp.GetAccountsAsync();
            if (accounts.Any())
            {
                try
                {
                    await ClientApplication.PublicClientApp.RemoveAsync(accounts.FirstOrDefault());

                    this.ResultText.Text = "User has signed-out";
                    this.SignInButton.Visibility = Visibility.Visible;
                    this.SignOutButton.Visibility = Visibility.Collapsed;
                    this.IdentityButton.Visibility = Visibility.Collapsed;
                }
                catch (MsalException ex)
                {
                    ResultText.Text = $"Error signing-out user: {ex.Message}";
                }
            }
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
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(idToken);
            var userClaims = token.Claims;

            var roles = await ApiService.GetRolesAsync(accessToken);

            var claimList = new List<Claim>();
            foreach (var role in roles)
            {
                claimList.Add(new Claim("role", role));
            }

            claimList.AddRange(userClaims);
            SetThreadPrincipal(claimList);

            TokenInfoText.Text = "";
            TokenInfoText.Text += $"Name: {userClaims.GetClaimValue("name")}" + Environment.NewLine;
            TokenInfoText.Text += $"User Identifier: {userClaims.GetClaimValue("oid")}" + Environment.NewLine;
            TokenInfoText.Text += $"Identity Provider: {userClaims.GetClaimValue("iss")}" + Environment.NewLine;
        }

        private static void SetThreadPrincipal(IEnumerable<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, "AAD", "name", "role");
            var principal = new ClaimsPrincipal(identity);

            AppDomain.CurrentDomain.SetThreadPrincipal(principal);
        }

        private void IdentityButton_Click(object sender, RoutedEventArgs e)
        {
            var identity = Thread.CurrentPrincipal.Identity;
            MessageBox.Show(identity?.Name);

            var hasRole1 = Thread.CurrentPrincipal.IsInRole("role1");
            MessageBox.Show(hasRole1 ? "Has Role 1" : "Does not have Role 1");
        }

        private void UseWam_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SignOutButton_Click(sender, e);
            App.CreateApplication(); // Not Azure AD accounts (that is use WAM accounts)
        }
    }
}
