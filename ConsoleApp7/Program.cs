using IdentityModel.Client;
using System;
using System.Net.Http;

namespace ConsoleApp7
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {

            var client = new HttpClient();

            var response = await client.RequestTokenAsync(new TokenRequest
            {
                Address = "http://localhost:5000/connect/token",
                GrantType = "client_credentials",

                ClientId = "client",
                ClientSecret = "secret",

                Parameters =
    {
        { "custom_parameter", "custom value"},
        { "scope", "api1" }
    }
            });
            var r = response.AccessToken;

            var client2 = new HttpClient();

            client2.SetBearerToken(r);
            var response2 = await client2.GetAsync("http://localhost:5001/WeatherForecast");

            Console.WriteLine("Hello World!");
        }
    }
}
