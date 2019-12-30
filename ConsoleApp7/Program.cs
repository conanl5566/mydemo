using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    internal class Program
    {
        private static async System.Threading.Tasks.Task Main(string[] args)
        {
            var response = await RequestTokenAsync();
            var r = response.AccessToken;

            var client2 = new HttpClient();

            client2.SetBearerToken(r);
            var response2 = await client2.GetStringAsync("http://localhost:5001/WeatherForecast");

            Console.WriteLine(response2);
            Console.ReadKey();
        }

        private static async Task<TokenResponse> RequestTokenAsync()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError) throw new Exception(disco.Error);

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret"
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }
    }
}