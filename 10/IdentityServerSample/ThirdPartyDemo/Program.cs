using System;
using System.Net.Http;
using IdentityModel.Client;
using static IdentityModel.OidcConstants;

namespace ThirdPartyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //1、根据entpoint获取token
            var diso = DiscoveryClient.GetAsync("http://localhost:5000").Result;
            if(diso.IsError)
            {
                Console.WriteLine(diso.Error);
            }
            var tokenClient = new TokenClient(diso.TokenEndpoint,"client","secret");
            var tokenResponse = tokenClient.RequestClientCredentialsAsync("api").Result;
            if(tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }
            else
            {
                Console.WriteLine(tokenResponse.Json);
            }
            var httpClient = new HttpClient();
            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var response = httpClient.GetAsync("http://localhost:5001/api/values").Result;
            Console.WriteLine(response.Content);

            Console.ReadLine();
        }
    }
}
