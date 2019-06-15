using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvcCookieAuthSample
{
    public class Config
    {
        /// <summary>
        /// 定义要保护的API资源，提供外部可访问的API资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1","api Application")
            };
        }

        /// <summary>
        /// 针对用户密码验证方式需要比对的账户
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "10000",
                    Username = "mark",
                    Password = "password"
                }
            };
        }

        /// <summary>
        /// 这个方法是来规范token生成的规则和方法的，（一般不进行设置，直接采用默认的即可，这里做了一下设置）
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        /// <summary>
        /// 客户端注册，定义客户端允许访问的API资源（作用域名称和验证规则）
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "Mvc Client",
                    ClientUri = "http://localhost:5001",
                    LogoUri ="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1560598257344&di=56109a9e38af3d5b6ef8550ba198de97&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201502%2F06%2F20150206231809_LH8SW.thumb.700_0.jpeg",
                    AllowRememberConsent = true,
                    //授权方式为Implicit，类型为GrantType枚举
                    AllowedGrantTypes ={ GrantType.Implicit },
                    //授权密钥，客户端和服务端
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    //禁用权限确认
                    //IdentityServer4 在登录完成的适合，会再跳转一次页面（权限确认）
                    //但实际业务场景并不需要进行权限确认，而是登陆成功后直接跳转到之前的页面就行了
                    //RequireConsent = false,
                    RequireConsent = true,
                    //oidc配置
                    RedirectUris = { "http://localhost:5001/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },

                    //允许客户端访问的Scopes[作用域]
                    //AllowedScopes = { "api1" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                    }
                }
            };
        }
    }
}
