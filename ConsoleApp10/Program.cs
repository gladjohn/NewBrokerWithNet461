using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        static async Task Main(string[] args)
        {
            string[] scopes = new[]
                {
                    "https://management.core.windows.net//.default"
                };

            IAccount accountToLogin = PublicClientApplication.OperatingSystemAccount;

            IntPtr consoleWindowHandle = GetConsoleWindow();
            Func<IntPtr> consoleWindowHandleProvider = () => consoleWindowHandle;

            PublicClientApplicationBuilder pcaBuilder = PublicClientApplicationBuilder
               .Create("04f0c124-f2bc-4f59-8241-bf6df9866bbd")
               .WithParentActivityOrWindow(consoleWindowHandleProvider)
               .WithAuthority("https://login.microsoftonline.com/organizations");

            IPublicClientApplication pca = pcaBuilder.WithBrokerPreview().Build();

            // Act
            try
            {
                var result = await pca.AcquireTokenSilent(scopes, accountToLogin).ExecuteAsync().ConfigureAwait(false);
                Console.WriteLine(result.AccessToken);
                Console.Read();
            }
            catch (MsalUiRequiredException ex)
            {

                var result = await pca.AcquireTokenInteractive(scopes)
                    .WithAccount(accountToLogin)
                    .ExecuteAsync()
                    .ConfigureAwait(false);

                Console.WriteLine(result.AccessToken);
                Console.Read();
            }
        }
    }
}
