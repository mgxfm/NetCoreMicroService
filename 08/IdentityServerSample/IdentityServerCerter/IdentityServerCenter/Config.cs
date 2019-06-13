using System;
using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServerCenter
{
    public class Config
    {
        public static List<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource("api","my_api")
            };
        }

        public static IEnumerable<Client> GetClient()
        {
            return new List<Client>
            {
                new Client 
                {
                   ClientId = "client",
                   AllowedGrantTypes = { GrantType.ClientCredentials},
                   ClientSecrets = { new Secret("secret".Sha256()) },
                   AllowedScopes = { "api" }
                }
            };
        }
    }
}
