using System;
using IdentityModel.Client;
using System.Net.Http;

namespace PwdClient
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
                return;
            }
            // 1-1、根据entPoint、ClientId和secret初始化tokenClient
            var tokenClient = new TokenClient(diso.TokenEndpoint,"pwdClient","secret");
            // 1-2 根据用户名、密码和ApiScope获取token
            var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync("mark","123456","api").Result;
            if(tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            else
            {
                Console.WriteLine(tokenResponse.AccessToken);
            }

            // 2、携带token访问资源api
            var httpClient = new HttpClient();
            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var response =  httpClient.GetAsync("http://localhost:5001/api/values").Result;

            if(response.IsSuccessStatusCode)
            {
                //打印api接口结果
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }

            Console.ReadLine();
        }
    }
}
