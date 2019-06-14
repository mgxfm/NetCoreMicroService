using System;
using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

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

        public static List<TestUser> GetTestUser()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                   SubjectId = "1",
                   Username = "mark",
                   Password = "123456"
                }
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
                },
                new Client
                {
                    ClientId = "pwdClient",
                    AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                    ClientSecrets = { new Secret("secret".Sha256())},
                    AllowedScopes = { "api" }
                }
            };
        }
    }
}
