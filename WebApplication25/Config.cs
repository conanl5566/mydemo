using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace WebApplication25
{
    /// <summary>
    /// Idnetity配置，初始化Identityserver
    /// </summary>
    public class Config
    {
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="1",
                    Username="lmc",
                    Password="123456"
                }
            };
        }

        //定义要保护的资源（webapi）
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("a", "WeatherForecast")
                {
                    //ApiSecrets=
                    //{
                    //    new Secret("secret".Sha256())
                    //},
                }
            };
        }

        //定义可以访问该API的客户端
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,  //设置模式，客户端模式
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "a" }
                },

                  new Client()
                {
                    ClientId="pwdClient",
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword, //密码模式
                    ClientSecrets= {new Secret("secret".Sha256()) },
                    AllowedScopes= { "a" }
                }
            };
        }
    }
}