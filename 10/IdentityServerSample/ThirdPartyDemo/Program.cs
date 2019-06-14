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
            // 1、向授权服务器获取Token
            var diso = DiscoveryClient.GetAsync("http://localhost:5000").Result;
            if(diso.IsError)
            {
                Console.WriteLine(diso.Error);
            }
            // 1-1、根据entpoint获取token
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
            //2、携带token访问资源api
            var response = httpClient.GetAsync("http://localhost:5001/api/values").Result;
            Console.WriteLine(response.Content);

            Console.ReadLine();
        }
    }
}
